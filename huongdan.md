# NỘI DUNG VĂN BẢN TRONG ĐỀ TÀI

1. **Backup & Restore DB:** cho phép người dùng có thể phục hồi cơ sở dữ liệu về :
- Thời điểm đã có bản backup;
- Thời điểm chưa có bản backup

Khi có sự cố trên cơ sở dữ liệu (thời điểm t₂) mà bản sao lưu mới nhất lại cách thời điểm đó vài tuần ( thời điểm t₁ , t₁< t₂ ) thì làm cách nào ta có thể phục hồi cơ sở dữ liệu về thời điểm t (t₁ < t < t₂) trước khi xảy ra sự cố.

✓ Chọn cơ sở dữ liệu cần thao tác trong danh sách bên trái;
✓ Nút lệnh Tạo device sao lưu: Ta phải tạo device trước thì mới được Sao lưu và Phục hồi cơ sở dữ liệu. Chương trình sẽ tự động tạo device tên theo dang sau: DEVICE_TENCSDL;
✓ Nút lệnh Sao lưu : tạo 1 bản backup cơ sở dữ liệu Full;
✓ Phục hồi: có 2 dang
- ■ Phục hồi cơ sở dữ liệu về bản backup mà ta đã sao lưu
- ■ Nếu ta chọn thêm checkbox Tham số phục hồi theo thời gian thì sẽ phục hồi cơ sở dữ liệu về thời điểm do ta nhập vào

---

# CHI TIẾT DỮ LIỆU TRONG HÌNH ẢNH GIAO DIỆN (SCREENSHOT)

## 1. Thành phần thanh tiêu đề và thanh menu lệnh
* **Tiêu đề cửa sổ:** Sao lưu - Phục hồi cơ sở dữ liệu trong SQL SERVER
* **Các nút chức năng trên thanh công cụ:**
  * `Sao lưu` (Biểu tượng quả cầu/ổ đĩa và mũi tên vàng)
  * `Phục hồi` (Biểu tượng ổ đĩa và mũi tên xanh ngược lại)
  * Checkbox: `Tham số phục hồi theo thời gian` (Có biểu tượng đồng hồ)
  * `Tạo device sao lưu` (Biểu tượng ổ đĩa kèm chữ device)
  * `Thoát` (Biểu tượng dấu X đỏ)

## 2. Danh sách Cơ sở dữ liệu (Khung bên trái)
Danh sách các database hiển thị trong cột **Cơ sở dữ liệu**:
* COICHAMTHI
* DIEMDANHSV
* distribution
* KEHOACHDAOTAO
* OnlineShop
* QL_VATTU *(Đang được chọn - có biểu tượng mũi tên đen chỉ vào)*
* QLDSV
* QLNXTP
* QLVANTHU
* QLVT
* QLVT_D16QT
* TEMP
* TRACNGHIEM
* TROIGIO

## 3. Khung hiển thị thông tin Bản sao lưu (Khung bên phải)
* **Tiêu đề bảng:** Tên cơ sở dữ liệu `QL_VATTU` | Giá trị bên cạnh: `3`
* **Các tiêu đề cột trong lưới dữ liệu:**
  * `Bản sao lưu thứ #`
  * `Diễn giải`
  * `Ngày giờ sao lưu`
  * `User sao lưu`

* **Chi tiết các dòng dữ liệu trong bảng (Lưới dữ liệu):**
  * **Dòng 1:** `3` | *(Để trống)* | `10/09/2018 5:59:04 PM` | `sa`
  * **Dòng 2:** `2` | *(Để trống)* | `08/09/2018 2:38:49 PM` | `sa`
  * **Dòng 3:** `1` | *(Để trống)* | `08/09/2018 10:32:48 AM` | `sa`

* **Tùy chọn dưới lưới dữ liệu:**
  * Checkbox: `Xóa tất cả các bản sao lưu cũ trong File trước khi sao lưu bản mới`

---

# THÀNH PHẦN GIAO DIỆN XUẤT HIỆN THÊM KHI TÍCH CHỌN THAM SỐ THỜI GIAN (HÌNH 2)

Khi checkbox **Tham số phục hồi theo thời gian** được tích chọn `[x]`, phần bên dưới giao diện xuất hiện thêm các thành phần sau:

* **Mục chọn thời gian:**
  * Nhãn: `Ngày giờ để phục hồi tới thời điểm đó`
  * Ô chọn ngày: `23/09/2018` (Có nút mũi tên xuống để chọn lịch)
  * Ô chọn giờ: `4:13:01 PM` (Có nút tăng giảm thời gian)

* **Khung văn bản Hướng dẫn:**
  * Tiêu đề khung: `Hướng dẫn:`
  * Nội dung chi tiết: `Ngày giờ ta nhập vào là thời điểm ta muốn phục hồi cơ sở dữ liệu về đó. Thời điểm này phải sau thời điểm của bản sao lưu mà ta đã chọn trên lưới, và trước thời điểm hiện tại ít nhất là 1 phút`