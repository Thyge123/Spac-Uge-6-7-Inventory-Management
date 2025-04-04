
import { getProducts } from '../api/client';
import { useQuery } from '@tanstack/react-query';

export default function Products() {

    const { data: products } = useQuery({ queryKey: ['products'], queryFn: () => getProducts() });

    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">Products</h1>
            <table className="w-full table-auto border">
                <thead>
                    <tr>
                        <th>ID</th><th>Name</th><th>Price</th><th>Category ID</th>
                    </tr>
                </thead>
                <tbody>
                    {products.map(p => (
                        <tr key={p.productId}>
                            <td>{p.productId}</td>
                            <td>{p.productName}</td>
                            <td>${p.price}</td>
                            <td>{p.categoryId}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}