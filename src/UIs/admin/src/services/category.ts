// api/category.ts
import { toast } from 'react-toastify';
import { Category } from '../types/category';
import { Response } from '../types/response';
import { apiFetch } from './api-interceptor';

const API_URL = import.meta.env.VITE_CORE_API_URL || '';

export const fetchCategories = async (): Promise<Category[]> => {
  const res = await apiFetch(`${API_URL}/api/categories`);
  var body = await res.json();
  const data = body.data as Category[];
  // Sort categories to show active ones first
  if (data && data.length > 0) {
    data.sort((a, b) => {
      if (a.status === 'Active' && b.status !== 'Active') return -1;
      if (a.status !== 'Active' && b.status === 'Active') return 1;
      return 0;
    });
  }
  return body.data;
  // return [
  //   {
  //     id: 'indoor',
  //     name: 'Trong nhà',
  //     slug: 'trong-nha',
  //     description: 'Sản phẩm trang trí và tiện ích cho không gian sống',
  //     subcategories: [
  //       {
  //         id: 'wind-chime',
  //         name: 'Chuông gió',
  //         slug: 'chuong-gio',
  //         description: 'Trang trí và tạo âm thanh thư giãn',
  //       },
  //       {
  //         id: 'lamp',
  //         name: 'Đèn',
  //         slug: 'den',
  //         description: 'Chiếu sáng và trang trí phòng',
  //       },
  //       {
  //         id: 'figure',
  //         name: 'Firgure',
  //         slug: 'firgure',
  //         description: 'Mô hình trang trí độc đáo',
  //       },
  //       {
  //         id: 'wood-shelf',
  //         name: 'Kệ gỗ',
  //         slug: 'ke-go',
  //         description: 'Lưu trữ và trang trí không gian',
  //       },
  //       {
  //         id: 'essential-oil',
  //         name: 'Tinh dầu',
  //         slug: 'tinh-dau',
  //         description: 'Hương thơm thư giãn cho phòng',
  //       },
  //       {
  //         id: 'wall-decal',
  //         name: 'Dán tường',
  //         slug: 'dan-tuong',
  //         description: 'Trang trí tường bằng hình dán',
  //       },
  //       {
  //         id: 'floor-decal',
  //         name: 'Dán sàn',
  //         slug: 'dan-san',
  //         description: 'Trang trí và bảo vệ sàn nhà',
  //       },
  //       {
  //         id: 'speaker',
  //         name: 'Loa',
  //         slug: 'loa',
  //         description: 'Thiết bị âm thanh cho giải trí',
  //       },
  //     ],
  //   },
  //   {
  //     id: 'accessories',
  //     name: 'Phụ kiện',
  //     slug: 'phu-kien',
  //     description: 'Các phụ kiện thời trang và tiện ích cá nhân',
  //     subcategories: [
  //       {
  //         id: 'watch',
  //         name: 'Đồng hồ',
  //         slug: 'dong-ho',
  //         description: 'Sang xịn mịn',
  //       },
  //       {
  //         id: 'earphones',
  //         name: 'Tai nghe',
  //         slug: 'tai-nghe',
  //         description: 'Nghe nhạc, gọi điện tiện lợi',
  //       },
  //       {
  //         id: 'glasses',
  //         name: 'Kính',
  //         slug: 'kinh',
  //         description: 'Bảo vệ mắt và thời trang',
  //       },
  //     ],
  //   },
  //   {
  //     id: 'cat',
  //     name: 'Cat',
  //     slug: 'cat',
  //     description: 'Đồ dùng và thức ăn cho thú cưng yêu quý',
  //     subcategories: [
  //       {
  //         id: 'cat-food',
  //         name: 'Thức ăn',
  //         slug: 'thuc-an',
  //         description: 'Dinh dưỡng đầy đủ cho mèo',
  //       },
  //       {
  //         id: 'scratch-board',
  //         name: 'Bàn cào',
  //         slug: 'ban-cao',
  //         description: 'Giúp mèo mài móng',
  //       },
  //       {
  //         id: 'brush',
  //         name: 'Bàn chải',
  //         slug: 'ban-chai',
  //         description: 'Chải lông giúp mèo sạch sẽ',
  //       },
  //       {
  //         id: 'litter',
  //         name: 'Cát',
  //         slug: 'cat',
  //         description: 'Vệ sinh cho mèo tiện lợi',
  //       },
  //     ],
  //   },
  // ];
};

export const addCategory = async (
  category: Category,
): Promise<Response<number>> => {
  const res = await apiFetch(`${API_URL}/api/categories`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(category),
  });

  if (res.status === 201) {
    toast.success('Thêm danh mục thành công!');
  }
  const body = await res.json();
  return body.data;
};

export const updateCategory = async (
  category: Category,
): Promise<Response<number>> => {
  const res = await apiFetch(`${API_URL}/api/categories`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(category),
  });

  if (res.status === 200) {
    toast.success('Cập nhật danh mục thành công!');
  }
  const body = await res.json();
  return body.data;
};

export const updateCategoryStatus = async (
  id: string,
  status: 'Active' | 'Deleted',
): Promise<boolean> => {
  const res = await apiFetch(`${API_URL}/api/categories/status`, {
    method: 'PATCH',
    body: JSON.stringify({ id, status }),
  });

  if (res.status === 200) {
    toast.success('Cập nhật trạng thái danh mục thành công!');
    return true;
  }
  return false;
};
