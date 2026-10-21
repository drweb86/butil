[Languages](README.md)

# Chính sách quyền riêng tư

Cập nhật lần cuối: 21 tháng 10 năm 2026

**BUtil** by Siarhei Kuchuk

Tên ứng dụng: BUtil
Tên nhà phát triển: Siarhei Kuchuk

BUtil sao lưu, đồng bộ và khôi phục tệp trên thiết bị này. Ứng dụng cũng có thể nhập phương tiện, chia sẻ một thư mục qua FTPS, hoặc tải tệp lên máy chủ do bạn cấu hình. Ứng dụng không tạo tài khoản nhà phát triển. Nhà phát triển không vận hành hệ thống phía sau để nhận tệp, mật khẩu hoặc dữ liệu sử dụng của bạn.

## Dữ liệu nhà phát triển không thu thập

Ứng dụng không có quảng cáo, phân tích, trình báo cáo sự cố hoặc SDK theo dõi. Nhà phát triển không thu thập, bán hoặc chia sẻ dữ liệu cá nhân.

Chính sách quyền riêng tư này và giấy phép được hiển thị bên trong ứng dụng. Chúng không được mở dưới dạng trang web.

## Dữ liệu lưu trên thiết bị của bạn

### Tác vụ và cài đặt

Định nghĩa tác vụ chỉ được lưu trong bộ nhớ riêng của ứng dụng trên thiết bị này. Một tác vụ có thể gồm đường dẫn thư mục, lịch, cài đặt lưu trữ, cùng mật khẩu hoặc mã thông báo bạn nhập. Mật khẩu và bí mật lưu trữ được niêm phong bằng kho khóa Android trước khi lưu, và chỉ đọc được trên thiết bị này. Những giá trị đó không được tải lên nhà phát triển. Chúng không nằm trong bản sao lưu Android. Gỡ ứng dụng sẽ xóa chúng.

Giao diện bạn chọn được lưu trong cài đặt riêng của ứng dụng trên thiết bị này.

### Tệp bạn chọn

Sao lưu, đồng bộ, khôi phục, nhập và tải lên sẽ đọc và ghi các thư mục bạn chọn bằng bộ chọn tài liệu của hệ thống, hoặc máy chủ bạn cấu hình. Những tệp đó ở trong thư mục bạn đã chọn, hoặc tại đích lưu trữ bạn cấu hình. Ứng dụng không tải chúng lên nhà phát triển. Gỡ ứng dụng không xóa tệp trong các thư mục bạn đã chọn.

### Chứng chỉ FTPS

Khi bạn chia sẻ một thư mục qua FTPS, ứng dụng tạo chứng chỉ tự ký trong bộ nhớ riêng của ứng dụng. Gỡ ứng dụng sẽ xóa chứng chỉ đó. Tệp được chia sẻ vẫn ở trong thư mục bạn đã cấp quyền.

Không dùng máy chủ của nhà phát triển để lưu dữ liệu của bạn.

## Sử dụng mạng

### Đích bạn cấu hình

Khi một tác vụ chạy, ứng dụng chỉ kết nối tới nơi bạn đặt. Đó có thể là một thư mục trên thiết bị này, hoặc một máy chủ bạn nhập: FTP, FTPS, SFTP, WebDAV, SMB, NFS, lưu trữ tương thích S3, hoặc Azure Blob Storage. Tên tệp, nội dung tệp và thông tin đăng nhập bạn đã nhập được gửi tới máy chủ đó để tác vụ chạy được. Mỗi dịch vụ đó có chính sách quyền riêng tư riêng. Nhà phát triển không nhận lưu lượng đó.

Chia sẻ thư mục qua FTPS lắng nghe trên thiết bị này để máy khách FTPS do bạn cấu hình có thể đọc hoặc ghi thư mục bạn đã cấp quyền, bằng tên người dùng và mật khẩu bạn đặt. Lưu lượng đó ở giữa các thiết bị của bạn (và bất kỳ ai trên mạng có những thông tin đăng nhập đó). Nhà phát triển không phải một bên của lưu lượng đó. Bạn chịu trách nhiệm về ai có thể tới cổng, thư mục nào bạn chia sẻ, và mật khẩu đó mạnh đến mức nào.

Ứng dụng không kiểm tra bản cập nhật và không liên hệ dịch vụ cập nhật.

## Lịch

Bạn có thể chạy một tác vụ khi thiết bị khởi động, hoặc vào những ngày bạn chọn. Ứng dụng đăng ký việc đó với đồng hồ báo thức của hệ thống. Việc đó chỉ khởi động ứng dụng này trên thiết bị của bạn.

## Quyền

Ứng dụng chỉ dùng các quyền sau:

- **Internet** và **trạng thái mạng** để tới máy chủ bạn cấu hình và để chia sẻ thư mục qua FTPS.
- **Thông báo** và một **dịch vụ nền trước** để tác vụ đang chạy tiếp tục khi màn hình tắt.
- **Báo thức chính xác** và **chạy khi khởi động** để bắt đầu tác vụ đã lên lịch.
- **Khóa đánh thức** để giảm ngủ trong lúc truyền.

## Trẻ em

Ứng dụng là công cụ sao lưu và đồng bộ tệp. Ứng dụng không hướng tới trẻ em dưới 13 tuổi.

## Bên thứ ba

Nhà cung cấp lưu trữ bạn cấu hình xử lý tệp và thông tin đăng nhập mà tác vụ gửi cho họ. Nhà phát triển không nhận lưu lượng đó.

## Thay đổi

Bản cập nhật chính sách này được đưa vào ứng dụng.

## Liên hệ

Tên ứng dụng: BUtil
Tên nhà phát triển: Siarhei Kuchuk

Câu hỏi về ứng dụng Android này có thể gửi qua trang liệt kê trên cửa hàng mà bạn dùng để cài đặt, hoặc tại [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
