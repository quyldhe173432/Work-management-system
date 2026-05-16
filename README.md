1. Giới thiệu chung
Dự án Work Management System là một giải pháp kỹ thuật số toàn diện được thiết kế nhằm tối ưu hóa hiệu suất làm việc, tăng cường sự hợp tác và hợp lý hóa việc phân bổ công việc trong các tổ chức, doanh nghiệp. Hệ thống tập trung vào việc quản lý dự án, theo dõi tiến độ và giao tiếp giữa các thành viên.

2. Các tính năng cốt lõi (Core Features)
Hệ thống được chia làm 3 nhóm tính năng chính tương ứng với các vai trò (Roles) trong một tổ chức:

Dành cho Ban Quản lý (Management Features):

Quản lý dự án (Project Management): Khởi tạo, chỉnh sửa và giám sát các dự án.

Ủy quyền công việc (Task Delegation): Giao việc trực tiếp cho các phòng ban hoặc cá nhân cụ thể.

Dành cho Nhân viên (Employee Features):

Nhận việc (Task Receipt): Xem danh sách công việc được giao, mức độ ưu tiên và thời hạn cụ thể.

Cập nhật tiến độ (Progress Updates): Thay đổi trạng thái công việc (Ví dụ: Đang làm, Đã hoàn thành) và đính kèm các tài liệu/kết quả liên quan.

Cảnh báo Deadline (Deadline Alerts): Nhận thông báo nhắc nhở về các công việc sắp đến hạn.

Tính năng Cộng tác & Hệ thống (Collaboration & System Features):

Không gian làm việc chung (Collaborative Workspaces): Nơi các thành viên có thể thảo luận, chia sẻ tài liệu trực tiếp dưới mỗi đầu việc.

Hệ thống thông báo (Notification System): Gửi cảnh báo tức thì khi có cập nhật mới về dự án, thay đổi trạng thái việc làm hoặc bình luận mới.

3. Công nghệ sử dụng (Tech Stack)
Dự án được xây dựng theo mô hình tách biệt Front-end và Back-end với các công nghệ phổ biến, mạnh mẽ:

Back-end: Sử dụng .NET Core (Web API) – mang lại hiệu năng cao, bảo mật tốt và dễ dàng mở rộng cấu trúc hệ thống.

Front-end: Sử dụng Next.js kết hợp với Tailwind CSS. Sự kết hợp này giúp tối ưu hóa tốc độ tải trang (giao diện mượt mà) và xây dựng giao diện responsive (tương thích tốt với cả máy tính lẫn điện thoại).

Cơ sở dữ liệu: SQL Server – hệ quản trị cơ sở dữ liệu quan hệ mạnh mẽ, đảm bảo tính toàn vẹn và an toàn cho dữ liệu của doanh nghiệp.

4. Kiến trúc hệ thống (Architecture)
Dự án được triển khai theo kiến trúc Clean Architecture (hoặc N-Tier) phân lớp rõ ràng nhằm tăng khả năng bảo trì và viết Unit Test:

Presentation Layer (API): Next.js UI tương tác với các API Endpoint của .NET Core.

Application Layer: Chứa các xử lý logic nghiệp vụ (Business Logic).

Domain Layer: Chứa các thực thể (Entities) cốt lõi của hệ thống.

Infrastructure Layer: Quản lý kết nối dữ liệu (SQL Server thông qua Entity Framework Core) và các dịch vụ bên ngoài (gửi mail, thông báo...).
