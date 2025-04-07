export type Product = {
    productId: number;
    productName?: string;
    price: number;
    category: ProductCategory;
};

export type ProductQueryParams = {
    isDescending: boolean;
    sortBy: string;
};

export type ProductCategory = {
    categoryId: number;
    categoryName: string;
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

export type ApiError = {
    message: string;
    code: string;
    status: number;
};

export type SidebarGroupArgs = {
    label: string;
    items?: {
        label: string;
        url: string;
    }[];
};