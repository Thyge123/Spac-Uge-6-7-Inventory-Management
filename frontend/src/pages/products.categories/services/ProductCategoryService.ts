import axiosClient from "@/api/axiosClient";
import { ProductCategoryEndpoints } from '@/api/endpoints';
import { type ProductCategory } from "@/types";

/**
 * Service class for handling product category-related API calls
 */
export default class ProductCategoryService {
    /**
     * Fetch all product categories with optional sorting and ordering
     * @returns Promise with all product category data
     */
    static async getAll(
    ): Promise<ProductCategory[]> {
        const response = await axiosClient.get(
            ProductCategoryEndpoints.getAll()
        );
        console.log(response.data);
        return response.data;
    }

    /**
     * Fetch a single product category by ID
     * @param id - Category ID
     * @returns Promise with product category data
     */
    static async getById(id: string): Promise<ProductCategory> {
        const response = await axiosClient.get(ProductCategoryEndpoints.getById(id));
        return response.data;
    }
}