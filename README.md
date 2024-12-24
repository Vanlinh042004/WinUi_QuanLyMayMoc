
** Hướng dẫn sử dụng:
- Hạn chế thao tác với Lõi và Linh kiện gốc
** Lĩnh fix
- Không còn xóa tất cả nha ae, tại dữ liệu gốc sao xóa tất cả được, nó k hợp lý cho lắm
- Một phần nữa là thêm xóa tất cả sẽ xuất hiện hiện tượng hai trường hợp trùng nhau + rất rất nhiều trường hợp
  + Xóa tất cả nhưng chưa lưu: dữ liệu chưa có trong bảng tạm + dự án tạm có rồi
  + Load lần đầu từ bảng gốc:  dữ liệu chưa có trong bảng tạm + dự án tạm có rồi
    => Xử lý được nhưng cần thêm 1 trường nữa vào bảng dự án + tạm => ảnh hưởng tới toàn code
    => Không cần thiết, Làm undo redo có vẻ nó cũng sẽ khó tại bên Lĩnh có 3 bảng lận
    
