export interface Product {
  productId: number;
  productName?: string;
  price: number;
  categoryId: number;
}

export interface OrderItemDto {
  productId: number;
  quantity: number;
}

export interface Order {
  orderId: number;
  customerId: number;
  orderDate: string;
  paymentMethod?: string;
  orderItems?: OrderItem[];
}

export interface OrderItem {
  orderItemId: number;
  orderId: number;
  productId: number;
  quantity: number;
}

export interface Category {
  id: number;
  name: string;
}
