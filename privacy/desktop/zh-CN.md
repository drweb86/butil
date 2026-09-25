[Languages](README.md)

# 隐私政策

最近更新：2026年9月25日

**BUtil** by Siarhei Kuchuk

应用程序名称：BUtil
开发者名称：Siarhei Kuchuk

BUtil 在这台计算机上备份、同步和还原文件。它也可以导入媒体、共享文件夹，或把文件上传到你配置的服务器。它不会创建开发者账户。开发者不运营接收你的文件、密码或使用数据的后端。

## 开发者不收集的数据

本应用不包含广告、分析、崩溃报告或跟踪 SDK。开发者不收集、出售或共享个人数据。

## 存储在你的计算机上的数据

### 任务和设置

任务定义只存储在这台计算机上。任务可以包含文件夹路径、计划、存储设置，以及你输入的密码或令牌。密码和存储机密在保存前会在这台计算机上加密，并且只能在这台计算机上读取。这些值不会上传给开发者。

- Windows 任务：`%AppData%\BUtil Backup Tasks`
- Linux 任务：`~/.config/BUtil Backup Tasks`
- Windows 设置（包括主题，以及上次为许可或隐私选择的语言）：`%AppData%\BUtil\Settings\v1`
- Linux 设置：`~/.config/BUtil/Settings/v1`
- Windows 任务状态：`%AppData%\BUtil\States`
- Linux 任务状态：`~/.config/BUtil/States`
- Windows 媒体导入状态：`%AppData%\BUtil Backup Tasks - States`
- Linux 媒体导入状态：`~/.config/BUtil Backup Tasks - States`

### 你选择的文件

备份、同步、还原和导入会读写你选择的文件夹。这些文件留在这台计算机上，或留在你配置的存储目标上。应用不会把它们上传给开发者。

### 日志

诊断日志只写在这台计算机上：

- Windows：`%LocalAppData%\BUtil\logs\v4`
- Linux：`~/.local/share/BUtil/logs/v4`

这些文件不会发送到任何地方。

不使用开发者的服务器来存储你的数据。

## 网络使用

### 你配置的目标

任务运行时，应用只连接到你设置的位置。可以是本地文件夹，或你输入的服务器：FTP、FTPS、SFTP、WebDAV、SMB、NFS、兼容 S3 的存储或 Azure Blob Storage。文件名、文件内容和你输入的凭据会发送到该服务器，以便任务能够运行。这些服务各自有自己的隐私政策。开发者不会收到该流量。

BUtil Server 可以在这台计算机上监听，以便你配置的 BUtil 客户端发送文件。该流量只在你设置的计算机之间传输。

### 更新检查

非 Store 版本可能会请求最新的 GitHub 版本：

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub（Microsoft）会收到一次普通的 HTTPS 请求（IP 地址、user-agent、时间）。开发者不会收到该流量。

从 Microsoft Store 安装的版本不使用此检查；更新由 Store 提供。

### 你打开的链接

应用可以在系统浏览器中打开这些页面。这些网站有自己的隐私政策：

- 项目主页：[github.com/drweb86/butil](https://github.com/drweb86/butil)
- 最新版本：[github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- 文件模式帮助：[learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- 日期格式帮助：[learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- 图标出处：[github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

许可和本隐私政策在应用内显示。它们不会作为网页打开。

## 计划

在 Windows 上，你可以在登录时或按每周计划运行任务。应用会在 Windows 任务计划程序中注册该任务，名称以 `BUtil` 开头。这只会在你的计算机上启动本应用。

## 儿童

本应用是备份和文件同步工具。它并非面向 13 岁以下儿童。

## 第三方

GitHub 会按上文所述处理更新检查请求和你打开的页面。Microsoft Store 处理 Store 安装和更新。你配置的存储提供商会处理任务发送给他们的文件和凭据。开发者不会收到该流量。

## 变更

本政策的更新将发布在项目存储库的此文件中。

## 联系方式

应用程序名称：BUtil
开发者名称：Siarhei Kuchuk

问题：[github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
