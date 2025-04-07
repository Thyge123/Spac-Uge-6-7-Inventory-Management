import { useQuery } from '@tanstack/react-query';
import type { ProductQueryParams } from '../../../types';
import { queryKeys } from '../../../api/queryClient';
import ProductService from '../services/ProductService';

export const useProducts = (params?: ProductQueryParams) => {
    return useQuery({
        queryKey: [...queryKeys.products.all, params],
        queryFn: () => ProductService.getAll(params),
    });
};