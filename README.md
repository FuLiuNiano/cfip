# CFip 节点助手

基于开源工具 [nodesCatch](https://github.com/bulianglin/nodescatch)(不良林)反编译还原并二次开发的免费节点测速工具,新增 **反代节点生成器**。

**仅供学习交流,请于下载后 24 小时内删除,请遵守当地法律法规。**

## 功能

- 原版功能:抓取公共免费节点、批量延迟/下载测速、订阅转换(subconverter)、Clash/Xray 双内核、一键自动测速
- **新增:反代节点生成器** —— 输入原始节点(vmess/vless/trojan)+ CDN 优选 IP 列表,批量生成把连接地址替换为优选 IP、Host/SNI 保留原域名的节点链接,可导出后直接导入测速

  ![icon](assets/icon.png)

- 附带网页版生成器 `web/proxygen.html`(浏览器直接打开使用,无需本程序)

## 构建

```
dotnet build src/nodesCatch.csproj -c Release
# 产物: src/bin/Release/net48/CFip.exe
```

依赖:.NET SDK 8+(目标框架 net48)。第三方库已在 `libs/` 内,无需还原 NuGet 包。

## 运行依赖

本程序在运行时需要以下组件,请自行下载后放到程序目录:

- **subconverter**:放到 `subconverter\subconverter.exe`(官方发布 https://github.com/tindy2013/subconverter )
- **clash 内核**:文件名 `clash-nodes.exe`(官方 https://github.com/Dreamacro/clash )
- **xray 内核**:文件名 `xray-nodes.exe`(官方 https://github.com/XTLS/Xray-core )

## 已知问题修复(相对原版)

- 修复原版向 subconverter 写入 `temp.txt` 时带 UTF-8 BOM 导致 subconverter 返回 400(No nodes were found!)的问题:附带的 `tools/bom_proxy.py` 会在 25500 端口剥除 BOM 后转发给 25501 的 subconverter,`tools/启动助手.bat` 一键拉起
- 反代生成器支持 vmess(标准 base64 JSON)、vless、trojan 三种链接,IP 带端口时同步替换端口

## 隐私

- 本程序不收集、不上传任何用户数据;配置与节点列表仅保存在本机
- 程序联网仅用于:下载订阅/节点源、节点测速(gstatic 204 探测、测速文件)、检测更新(原作者 GitHub)

## 致谢

- [nodesCatch](https://github.com/bulianglin/nodescatch) — 原工具作者(不良林)
- [v2rayN](https://github.com/2dust/v2rayN) / [subconverter](https://github.com/tindy2013/subconverter) / [Clash](https://github.com/Dreamacro/clash) / [Xray-core](https://github.com/XTLS/Xray-core)
