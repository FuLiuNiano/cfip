# -*- coding: utf-8 -*-
"""
nodesCatch 助手代理:
  监听 127.0.0.1:25500(nodesCatch 硬编码调用的端口),
  转发请求给真正的 subconverter(127.0.0.1:25501),
  转发前剥掉 temp.txt 的 UTF-8 BOM —— BOM 会让 subconverter 的
  base64 解码失败,返回 400 "No nodes were found!"。
"""
import os
import urllib.request
import urllib.parse
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer

LISTEN = ("127.0.0.1", 25500)
BACKEND = "http://127.0.0.1:25501"
TEMP = os.path.join(os.path.dirname(os.path.abspath(__file__)),
                    "subconverter", "temp.txt")
LOG = os.path.join(os.path.dirname(os.path.abspath(__file__)), "bom_proxy.log")


def log(msg):
    try:
        with open(LOG, "a", encoding="utf-8") as f:
            from datetime import datetime
            f.write(datetime.now().strftime("%Y-%m-%d %H:%M:%S ") + msg + "\n")
    except OSError:
        pass


def strip_bom():
    """temp.txt 存在且带 BOM 时,原子重写为无 BOM 版本。"""
    if not os.path.isfile(TEMP):
        return
    with open(TEMP, "rb") as f:
        data = f.read()
    if data[:3] == b"\xef\xbb\xbf":
        tmp = TEMP + ".tmp"
        with open(tmp, "wb") as f:
            f.write(data[3:])
        os.replace(tmp, TEMP)
        log("stripped BOM from temp.txt (%d bytes)" % (len(data) - 3))


class Handler(BaseHTTPRequestHandler):
    def do_GET(self):
        q = urllib.parse.urlparse(self.path).query
        if "url=" in q and "temp.txt" in urllib.parse.unquote(q):
            strip_bom()
        dest = BACKEND + self.path
        req = urllib.request.Request(dest)
        for h in ("Content-Type", "Authorization"):
            v = self.headers.get(h)
            if v:
                req.add_header(h, v)
        try:
            with urllib.request.urlopen(req, timeout=120) as r:
                body, code, ctype = r.read(), r.status, r.headers.get("Content-Type", "")
        except urllib.error.HTTPError as e:
            body, code, ctype = e.read(), e.code, e.headers.get("Content-Type", "")
        except Exception as e:
            body, code, ctype = ("proxy error: %s" % e).encode(), 502, "text/plain"
        self.send_response(code)
        self.send_header("Content-Type", ctype)
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    do_POST = do_GET

    def log_message(self, fmt, *args):
        log("%s %s" % (self.address_string(), fmt % args))


if __name__ == "__main__":
    log("proxy listening on %s -> %s" % (LISTEN, BACKEND))
    ThreadingHTTPServer(LISTEN, Handler).serve_forever()
