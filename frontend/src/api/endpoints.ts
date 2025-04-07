export const ProductEndpoints = {
    getAll: (params: string) => `/products${params ? `?${params}` : ""}`,
    getById: (id: string | number) => `/products/${id}`,
    // getProductsFromOrder: (orderToken: string) => `/product/order-products?token=${orderToken}`,
} as const;

export const ProductCategoryEndpoints = {
    getAll: () => `/categories`,
    getById: (id: string | number) => `/categories/${id}`,
    // getProductsFromOrder: (orderToken: string) => `/product/order-products?token=${orderToken}`,
} as const;