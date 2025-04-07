import { DetailView } from '@/components/ui/DetailView';
import { useProduct } from '@/pages/products/queries/ProductQueries';
import type { Product, ProductCategory } from '@/types';
import type { ColumnDef } from '@tanstack/react-table';
import { useNavigate } from 'react-router-dom';

export const ProductDetail: React.FC = () => {
    const navigate = useNavigate();

    const columns: ColumnDef<Product>[] = [
        {
            header: "Id",
            accessorKey: "productId"
        },
        {
            header: "Name",
            accessorKey: "productName"
        },
        {
            header: "Price",
            accessorKey: "price",
            cell: ({ row }) => {
                const amount = parseFloat(row.getValue("price"));
                const formatted = new Intl.NumberFormat("en-US", {
                    style: "currency",
                    currency: "USD",
                }).format(amount);

                return <div className="text-right font-medium">{formatted}</div>;
            },
        },
        {
            header: "Category",
            accessorKey: "category",
            cell: ({ row }) => {
                const { categoryId, categoryName }: ProductCategory = row.getValue("category");
                return (
                    <a onClick={() => navigate(`/products/categories/${categoryId}`)}>
                        {categoryId + " | " + categoryName}
                    </a>
                );
            }
        }
    ];

    return (
        <DetailView dataName="product" query={useProduct} columns={columns} />
    );
};