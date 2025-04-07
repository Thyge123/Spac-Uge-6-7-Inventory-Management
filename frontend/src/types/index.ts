export type Product = {
    productId: number;
    productName?: string;
    price: number;
    category: Category;
};

export type ProductQueryParams = {
    isDescending: boolean;
    sortBy: string;
};

export type OrderItemDto = {
    productId: number;
    quantity: number;
};

export type Order = {
    orderId: number;
    customerId: number;
    orderDate: string;
    paymentMethod?: string;
    orderItems?: OrderItem[];
};

export type OrderItem = {
    orderItemId: number;
    orderId: number;
    productId: number;
    quantity: number;
};

export type Category = {
    categoryId: number;
    categoryName: string;
};

export type BaseResponse<T> = {
    data: T[];
};