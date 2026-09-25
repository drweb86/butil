[Languages](README.md)

# Chính sách quyền riêng tư

Cập nhật lần cuối: 25 tháng 9 năm 2026

**BUtil** by Siarhei Kuchuk

Tên ứng dụng: BUtil
Tên nhà phát triển: Siarhei Kuchuk

BUtil sao lưu, đồng bộ và khôi phục tệp trên máy tính này. Ứng dụng cũng có thể nhập phương tiện, chia sẻ thư mục hoặc tải tệp lên máy chủ do bạn cấu hình. Ứng dụng không tạo tài khoản nhà phát triển. Nhà phát triển không vận hành máy chủ nhận tệp, mật khẩu hoặc dữ liệu sử dụng của bạn.

## Dữ liệu nhà phát triển không thu thập

Ứng dụng không có quảng cáo, phân tích, báo cáo sự cố hay SDK theo dõi. Nhà phát triển không thu thập, bán hoặc chia sẻ dữ liệu cá nhân.

## Dữ liệu được lưu trên máy tính của bạn

### Tác vụ và cài đặt

Định nghĩa tác vụ chỉ được lưu trên máy tính này. Một tác vụ có thể gồm đường dẫn thư mục, lịch, cài đặt lưu trữ và mật khẩu hoặc mã thông báo bạn nhập. Mật khẩu và bí mật lưu trữ được mã hóa trên máy tính này trước khi lưu, và chỉ đọc được trên máy tính này. Những giá trị đó không được tải lên cho nhà phát triển.

- Tác vụ Windows: `%AppData%\BUtil Backup Tasks`
- Tác vụ Linux: `~/.config/BUtil Backup Tasks`
- Cài đặt Windows (gồm chủ đề và ngôn ngữ được chọn gần nhất cho giấy phép hoặc quyền riêng tư): `%AppData%\BUtil\Settings\v1`
- Cài đặt Linux: `~/.config/BUtil/Settings/v1`
- Trạng thái tác vụ Windows: `%AppData%\BUtil\States`
- Trạng thái tác vụ Linux: `~/.config/BUtil/States`
- Trạng thái nhập phương tiện Windows: `%AppData%\BUtil Backup Tasks - States`
- Trạng thái nhập phương tiện Linux: `~/.config/BUtil Backup Tasks - States`

### Tệp bạn chọn

Sao lưu, đồng bộ, khôi phục và nhập đọc và ghi các thư mục bạn chọn. Những tệp đó ở lại trên máy tính này hoặc tại đích lưu trữ bạn cấu hình. Ứng dụng không tải chúng lên cho nhà phát triển.

### Nhật ký

Nhật ký chẩn đoán chỉ được ghi trên máy tính này:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Những tệp đó không được gửi đi đâu.

Không có máy chủ của nhà phát triển được dùng để lưu dữ liệu của bạn.

## Sử dụng mạng

### Đích bạn cấu hình

Khi một tác vụ chạy, ứng dụng chỉ kết nối tới nơi bạn đặt. Đó có thể là thư mục cục bộ hoặc máy chủ bạn nhập: FTP, FTPS, SFTP, WebDAV, SMB, NFS, lưu trữ tương thích S3 hoặc Azure Blob Storage. Tên tệp, nội dung tệp và thông tin đăng nhập bạn nhập được gửi tới máy chủ đó để tác vụ có thể chạy. Mỗi dịch vụ đó có chính sách quyền riêng tư riêng. Nhà phát triển không nhận lưu lượng đó.

BUtil Server có thể lắng nghe trên máy tính này để máy khách BUtil do bạn cấu hình có thể gửi tệp. Lưu lượng đó chỉ ở giữa các máy tính bạn thiết lập.

### Kiểm tra cập nhật

Bản dựng không từ Store có thể yêu cầu bản phát hành GitHub mới nhất:

`https://api.github.com/repos/drweb86/butil/releases/latest`

GitHub (Microsoft) nhận một yêu cầu HTTPS thông thường (địa chỉ IP, user-agent, thời gian). Nhà phát triển không nhận lưu lượng đó.

Bản cài từ Microsoft Store không dùng kiểm tra này; Store cung cấp bản cập nhật.

### Liên kết bạn mở

Ứng dụng có thể mở các trang này trong trình duyệt hệ thống. Các trang web đó có chính sách quyền riêng tư riêng:

- Trang chủ dự án: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Bản phát hành mới nhất: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Trợ giúp mẫu tệp: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Trợ giúp định dạng ngày: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Nguồn biểu tượng: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

Giấy phép và chính sách quyền riêng tư này được hiển thị trong ứng dụng. Chúng không được mở như trang web.

## Lịch

Trên Windows, bạn có thể chạy tác vụ khi đăng nhập hoặc theo lịch hàng tuần. Ứng dụng đăng ký việc đó với Bộ lập lịch tác vụ Windows bằng tên bắt đầu bằng `BUtil`. Việc này chỉ khởi chạy ứng dụng này trên máy tính của bạn.

## Trẻ em

Ứng dụng là công cụ sao lưu và đồng bộ tệp. Ứng dụng không hướng tới trẻ em dưới 13 tuổi.

## Bên thứ ba

GitHub xử lý yêu cầu kiểm tra cập nhật và các trang bạn mở, như mô tả ở trên. Microsoft Store xử lý cài đặt và cập nhật từ Store. Nhà cung cấp lưu trữ bạn cấu hình xử lý tệp và thông tin đăng nhập mà tác vụ gửi cho họ. Nhà phát triển không nhận lưu lượng đó.

## Thay đổi

Các bản cập nhật chính sách này sẽ được đăng trong tệp này tại kho lưu trữ dự án.

## Liên hệ

Tên ứng dụng: BUtil
Tên nhà phát triển: Siarhei Kuchuk

Câu hỏi: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
