[Languages](README.md)

# 私隱政策

最近更新：2026年9月25日

**BUtil** by Siarhei Kuchuk

應用程式名稱：BUtil
開發者名稱：Siarhei Kuchuk

BUtil 喺呢部電腦備份、同步同還原檔案。佢亦可以匯入媒體、共用資料夾，或者將檔案上傳到你設定嘅伺服器。佢唔會建立開發者帳戶。開發者冇營運接收你嘅檔案、密碼或者使用資料嘅後端。

## 開發者唔收集嘅資料

呢個應用程式冇廣告、分析、當機報告或者追蹤 SDK。開發者唔收集、唔出售、亦唔共享個人資料。

## 儲存喺你電腦嘅資料

### 工作同設定

工作定義只儲存喺呢部電腦。一項工作可以包括資料夾路徑、排程、儲存設定，同你輸入嘅密碼或者權杖。密碼同儲存密鑰喺儲存之前會喺呢部電腦加密，而且只可以喺呢部電腦讀取。呢啲值唔會上傳俾開發者。

- Windows 工作：`%AppData%\BUtil Backup Tasks`
- Linux 工作：`~/.config/BUtil Backup Tasks`
- Windows 設定（包括主題，同上次為授權或者私隱揀嘅語言）：`%AppData%\BUtil\Settings\v1`
- Linux 設定：`~/.config/BUtil/Settings/v1`
- Windows 工作狀態：`%AppData%\BUtil\States`
- Linux 工作狀態：`~/.config/BUtil/States`
- Windows 媒體匯入狀態：`%AppData%\BUtil Backup Tasks - States`
- Linux 媒體匯入狀態：`~/.config/BUtil Backup Tasks - States`

### 你揀嘅檔案

備份、同步、還原同匯入會讀寫你揀嘅資料夾。呢啲檔案留喺呢部電腦，或者留喺你設定嘅儲存目的地。應用程式唔會將佢哋上傳俾開發者。

### 記錄

診斷記錄只寫喺呢部電腦：

- Windows：`%LocalAppData%\BUtil\logs\v4`
- Linux：`~/.local/share/BUtil/logs/v4`

呢啲檔案唔會傳去任何地方。

唔會用開發者嘅伺服器儲存你嘅資料。

## 網絡使用

### 你設定嘅目的地

工作執行嗰陣，應用程式只會連接你設定嘅位置。可以係本機資料夾，或者你輸入嘅伺服器：FTP、FTPS、SFTP、WebDAV、SMB、NFS、相容 S3 嘅儲存或者 Azure Blob Storage。檔案名稱、檔案內容同你輸入嘅憑證會傳去該伺服器，等工作可以執行。呢啲服務各自有自己嘅私隱政策。開發者唔會收到該流量。

BUtil Server 可以喺呢部電腦聽候連線，等你設定嘅 BUtil 用戶端傳送檔案。該流量只會喺你設定嘅電腦之間傳送。

### 更新檢查

非 Store 版本可能會要求最新嘅 GitHub 版本：

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub（Microsoft）會收到一次普通嘅 HTTPS 要求（IP 位址、user-agent、時間）。開發者唔會收到該流量。

由 Microsoft Store 安裝嘅版本唔用呢項檢查；更新由 Store 提供。

### 你開啟嘅連結

應用程式可以喺系統瀏覽器開啟呢啲頁面。呢啲網站有自己嘅私隱政策：

- 專案主頁：[github.com/drweb86/butil](https://github.com/drweb86/butil)
- 最新版本：[github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- 檔案模式說明：[learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- 日期格式說明：[learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- 圖示出處：[github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

授權同本私隱政策喺應用程式入面顯示。佢哋唔會當成網頁開啟。

## 排程

喺 Windows，你可以喺登入時或者按每週排程執行工作。應用程式會喺 Windows 工作排程器登記該工作，名稱以 `BUtil` 開頭。呢樣只會喺你嘅電腦啟動本應用程式。

## 兒童

本應用程式係備份同檔案同步工具。佢唔係面向 13 歲以下兒童。

## 第三方

GitHub 會按上文所述處理更新檢查要求同你開啟嘅頁面。Microsoft Store 處理 Store 安裝同更新。你設定嘅儲存服務供應商會處理工作傳送俾佢哋嘅檔案同憑證。開發者唔會收到該流量。

## 變更

本政策嘅更新會發佈喺專案存放庫嘅呢個檔案。

## 聯絡

應用程式名稱：BUtil
開發者名稱：Siarhei Kuchuk

問題：[github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
