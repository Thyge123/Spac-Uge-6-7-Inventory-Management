export const ProductEndpoints = {
    getAll: (params: string) => `/products${params ? `?${params}` : ""}`,
    getById: (id: string | number) => `/products/${id}`,
    // getProductsFromOrder: (orderToken: string) => `/product/order-products?token=${orderToken}`,
} as const;