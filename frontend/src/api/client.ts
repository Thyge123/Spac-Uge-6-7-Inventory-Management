import axios from 'axios';
import type { Product } from '../types';

export const api = axios.create({
    baseURL: 'https://localhost:7117', // change as needed
    headers: {
        'Content-Type': 'application/json'
    }
});

export async function getProducts() {
    const response = await api.get<Product[]>('/api/Products');
    return response.data;
}