[Languages](README.md)

# 개인정보 처리방침

최종 업데이트: 2026년 9월 25일

**BUtil** by Siarhei Kuchuk

애플리케이션 이름: BUtil
개발자 이름: Siarhei Kuchuk

BUtil은 이 컴퓨터에서 파일을 백업하고, 동기화하고, 복원합니다. 미디어를 가져오거나, 폴더를 공유하거나, 사용자가 설정한 서버로 파일을 업로드할 수도 있습니다. 개발자 계정은 만들지 않습니다. 개발자는 파일, 암호 또는 사용 데이터를 받는 백엔드를 운영하지 않습니다.

## 개발자가 수집하지 않는 데이터

이 앱에는 광고, 분석, 충돌 보고, 추적 SDK가 없습니다. 개발자는 개인 데이터를 수집하거나 판매하거나 공유하지 않습니다.

## 이 컴퓨터에 저장되는 데이터

### 작업과 설정

작업 정의는 이 컴퓨터에만 저장됩니다. 작업에는 폴더 경로, 일정, 저장소 설정, 입력한 암호 또는 토큰이 포함될 수 있습니다. 암호와 저장소 비밀은 저장되기 전에 이 컴퓨터에서 암호화되며, 이 컴퓨터에서만 읽을 수 있습니다. 이 값은 개발자에게 업로드되지 않습니다.

- Windows 작업: `%AppData%\BUtil Backup Tasks`
- Linux 작업: `~/.config/BUtil Backup Tasks`
- Windows 설정(테마와 라이선스 또는 개인정보에서 마지막으로 선택한 언어 포함): `%AppData%\BUtil\Settings\v1`
- Linux 설정: `~/.config/BUtil/Settings/v1`
- Windows 작업 상태: `%AppData%\BUtil\States`
- Linux 작업 상태: `~/.config/BUtil/States`
- Windows 미디어 가져오기 상태: `%AppData%\BUtil Backup Tasks - States`
- Linux 미디어 가져오기 상태: `~/.config/BUtil Backup Tasks - States`

### 선택한 파일

백업, 동기화, 복원, 가져오기는 선택한 폴더를 읽고 씁니다. 그 파일은 이 컴퓨터 또는 설정한 저장소 대상에 남습니다. 앱은 그 파일을 개발자에게 업로드하지 않습니다.

### 로그

진단 로그는 이 컴퓨터에만 기록됩니다.

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

이 파일은 어디로도 전송되지 않습니다.

데이터를 저장하는 데 개발자 서버는 사용되지 않습니다.

## 네트워크 사용

### 설정한 대상

작업이 실행되면 앱은 설정한 위치에만 연결합니다. 로컬 폴더이거나 입력한 서버일 수 있습니다. FTP, FTPS, SFTP, WebDAV, SMB, NFS, S3 호환 저장소 또는 Azure Blob Storage입니다. 파일 이름, 파일 내용, 입력한 자격 증명은 작업이 실행될 수 있도록 해당 서버로 전송됩니다. 각 서비스에는 자체 개인정보 처리방침이 있습니다. 개발자는 그 트래픽을 받지 않습니다.

BUtil Server는 설정한 BUtil 클라이언트가 파일을 보낼 수 있도록 이 컴퓨터에서 수신 대기할 수 있습니다. 그 트래픽은 설정한 컴퓨터 사이에만 머뭅니다.

### 업데이트 확인

Store가 아닌 빌드는 최신 GitHub 릴리스를 요청할 수 있습니다.

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub(Microsoft)는 일반적인 HTTPS 요청(IP 주소, user-agent, 시각)을 받습니다. 개발자는 그 트래픽을 받지 않습니다.

Microsoft Store에서 설치한 경우에는 이 확인을 사용하지 않습니다. 업데이트는 Store가 제공합니다.

### 여는 링크

앱은 시스템 브라우저에서 다음 페이지를 열 수 있습니다. 이 사이트에는 자체 개인정보 처리방침이 있습니다.

- 프로젝트 홈페이지: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- 최신 릴리스: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- 파일 패턴 도움말: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- 날짜 형식 도움말: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- 아이콘 출처: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

라이선스와 이 개인정보 처리방침은 앱 안에 표시됩니다. 웹 페이지로 열리지 않습니다.

## 일정

Windows에서는 로그인할 때 또는 매주 일정으로 작업을 실행할 수 있습니다. 앱은 이름이 `BUtil`로 시작하는 작업으로 Windows 작업 스케줄러에 등록합니다. 이것은 이 컴퓨터에서 이 앱을 실행할 뿐입니다.

## 아동

이 앱은 백업 및 파일 동기화 도구입니다. 13세 미만 아동을 대상으로 하지 않습니다.

## 제3자

GitHub는 위에 설명한 대로 업데이트 확인 요청과 여는 페이지를 처리합니다. Microsoft Store는 Store 설치와 업데이트를 처리합니다. 설정한 저장소 제공자는 작업이 보내는 파일과 자격 증명을 처리합니다. 개발자는 그 트래픽을 받지 않습니다.

## 변경

이 방침의 업데이트는 프로젝트 저장소의 이 파일에 게시됩니다.

## 연락처

애플리케이션 이름: BUtil
개발자 이름: Siarhei Kuchuk

질문: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
