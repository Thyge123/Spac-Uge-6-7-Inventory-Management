import { useQuery, type UseQueryResult } from '@tanstack/react-query';
import type { ApiError, Product, ProductQueryParams } from '@/types';
import { queryKeys } from '@/api/queryClient';
import ProductService from '@/pages/products/services/ProductService';

export const useProducts = (params?: ProductQueryParams): UseQueryResult<Product[], ApiError> => {
    return useQuery({
        queryKey: [...queryKeys.products.all, params],
        queryFn: () => ProductService.getAll(params),
    });
};

export const useProduct = (id: string): UseQueryResult<Product, ApiError> => {
    return useQuery({
        queryKey: queryKeys.products.byId(id),
        queryFn: () => ProductService.getById(id)
    });
};