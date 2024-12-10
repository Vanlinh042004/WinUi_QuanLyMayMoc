Nhật test 08/12/2024:




Chức năng: DỊCH VỤ THEO THÁNG


+Lỗi: Nhập số điện thoại nhiều hơn 10 số bị crash chương trình 

Các trường manv, malinhkien, maloi được set là varchar(50), nên nhập hơn 50 là crash => Xử lý giao diện, thông báo cho người dùng, exception này kia cho nó không crash

Chức năng Tìm nhân viên không hoạt động => sửa lại để tìm được nhân viên
 

+Có thể cải thiện:

Cột "Ngày thực hiện" không hiện giờ phút giây, vì không lưu trữ giờ phút giây nên lúc nào nó cũng hiện "12:00:00AM" => Sửa lại StringFormat của Datetime
 
Bỏ cột yêu cầu nhập "Tên linh kiện", "Tên lõi", "Tên nhân viên". Tuy nhiên nếu bỏ thì đồng thời cần phải làm gợi ý Mã lõi, Mã linh kiện, Mã nhân viên.


Chức năng: LINH KIỆN

Bấm thêm linh kiện, điền thông tin, bấm lưu không thấy lưu xuống database => Cần phải lưu xuống database, xong mở lại dự án thì phải load lại được mấy cái mới thêm

Bấm sửa linh kiện, thay đổi thông tin, bấm lưu không thấy lưu xuống database => Tạm thời Nhật chưa biết sửa sao

Bấm xóa linh kiện không có thay đổi gì trong database => Tạm thời Nhật chưa biết sửa sao ==> Cần họp để bàn lại xử lý chỗ này
==============================================
=> Lĩnh fix lại hết ròi, thao tác với Linh Kiện sẽ lưu vào bảng linhkenduantam -> Lưu dự án thì lưu từ bảng tạm sang linhkien_duan, Nhật muốn mở dự án cũ thì load từ bảng linhkien_duan

Chức năng: LÕI	

Lõi được thêm mới thì có load xuống database, nhưng khi mở dự án thì không load từ database lên cái lõi mới được thêm đó => cần phải xử lý để khi

Xóa lõi, xóa tất cả không có thay đổi gì trong database, lần sau mở lên vẫn load lại toàn bộ những cái lõi mẫu => Tạm thời Nhật chưa biết sửa sao

================================================
Tương tự Linh Kiên
Chức năng: DANH SÁCH NHÂN VIÊN

Nhân viên ở dự án nào cũng được load => Xử lý lại để nó chỉ load nhân viên của dự án đang mở thoi


Chức năng: THỐNG KÊ MÁY MÓC

Hoạt động tốt


Chức năng: LƯU DỰ ÁN VỚI TÊN KHÁC

Hiện chưa copy nhân viên này qua nhân viên của bảng khác.

Hiện chưa copy dịch vụ theo tháng qua dự án khác.
