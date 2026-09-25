[Languages](README.md)

# プライバシーポリシー

最終更新日: 2026年9月25日

**BUtil** by Siarhei Kuchuk

アプリケーション名: BUtil
開発者名: Siarhei Kuchuk

BUtil は、このコンピューター上のファイルをバックアップ、同期、復元します。メディアの取り込み、フォルダーの共有、設定したサーバーへのファイルのアップロードもできます。開発者アカウントは作成しません。開発者は、ファイル、パスワード、利用状況を受け取るバックエンドを運用していません。

## 開発者が収集しないデータ

このアプリには、広告、分析、クラッシュ報告、追跡用 SDK は含まれていません。開発者は個人データを収集、販売、共有しません。

## このコンピューターに保存されるデータ

### タスクと設定

タスクの定義はこのコンピューターにだけ保存されます。タスクには、フォルダーのパス、スケジュール、ストレージ設定、入力したパスワードやトークンを含めることができます。パスワードとストレージの秘密情報は、保存前にこのコンピューター上で暗号化され、このコンピューターでのみ読み取れます。これらの値は開発者に送信されません。

- Windows のタスク: `%AppData%\BUtil Backup Tasks`
- Linux のタスク: `~/.config/BUtil Backup Tasks`
- Windows の設定（テーマ、およびライセンスまたはプライバシーで最後に選んだ言語を含む）: `%AppData%\BUtil\Settings\v1`
- Linux の設定: `~/.config/BUtil/Settings/v1`
- Windows のタスク状態: `%AppData%\BUtil\States`
- Linux のタスク状態: `~/.config/BUtil/States`
- Windows のメディア取り込み状態: `%AppData%\BUtil Backup Tasks - States`
- Linux のメディア取り込み状態: `~/.config/BUtil Backup Tasks - States`

### 選択したファイル

バックアップ、同期、復元、取り込みは、選択したフォルダーを読み書きします。これらのファイルは、このコンピューター、または設定した保存先に残ります。アプリはそれらを開発者にアップロードしません。

### ログ

診断ログはこのコンピューターにだけ書き込まれます。

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

これらのファイルはどこにも送信されません。

データを保存するために開発者のサーバーは使われません。

## ネットワークの利用

### 設定した保存先

タスクの実行時、アプリは設定した場所にだけ接続します。ローカルフォルダー、または入力したサーバーです。FTP、FTPS、SFTP、WebDAV、SMB、NFS、S3 互換ストレージ、Azure Blob Storage が使えます。ファイル名、ファイルの内容、入力した資格情報は、タスクを実行するためにそのサーバーへ送信されます。各サービスには独自のプライバシーポリシーがあります。開発者はその通信を受け取りません。

BUtil Server は、設定した BUtil クライアントがファイルを送れるように、このコンピューターで待ち受けることができます。その通信は、設定したコンピューターの間だけに留まります。

### 更新の確認

Store 以外のビルドは、最新の GitHub リリースを要求することがあります。

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub（Microsoft）は通常の HTTPS 要求（IP アドレス、user-agent、時刻）を受け取ります。開発者はその通信を受け取りません。

Microsoft Store からのインストールでは、この確認は行われません。更新は Store が配信します。

### 開くリンク

アプリは、システムのブラウザーで次のページを開くことができます。これらのサイトには独自のプライバシーポリシーがあります。

- プロジェクトのホームページ: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- 最新リリース: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- ファイルパターンのヘルプ: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- 日付形式のヘルプ: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- アイコンのクレジット: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

ライセンスとこのプライバシーポリシーはアプリ内に表示されます。Web ページとしては開かれません。

## スケジュール

Windows では、サインイン時または毎週のスケジュールでタスクを実行できます。アプリは、名前が `BUtil` で始まるタスクとして Windows タスク スケジューラに登録します。これは、このコンピューターでこのアプリを起動するだけです。

## 子ども

このアプリはバックアップとファイル同期のためのツールです。13 歳未満の子どもを対象としていません。

## 第三者

GitHub は、上記のとおり更新確認の要求と開いたページを処理します。Microsoft Store は Store からのインストールと更新を処理します。設定したストレージ事業者は、タスクが送るファイルと資格情報を処理します。開発者はその通信を受け取りません。

## 変更

このポリシーの更新は、プロジェクトのリポジトリにあるこのファイルに掲載されます。

## 連絡先

アプリケーション名: BUtil
開発者名: Siarhei Kuchuk

質問: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
