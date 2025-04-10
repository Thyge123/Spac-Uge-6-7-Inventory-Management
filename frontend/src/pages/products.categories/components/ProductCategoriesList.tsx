import { DataTable } from '@/components/ui/data-table';
import { useProductCategories } from '@/pages/products.categories/queries/ProductCategoryQueries';
import type { ProductCategory } from '@/types';
import type { ColumnDef } from '@tanstack/react-table';
import { Link } from 'react-router-dom';

const columns: ColumnDef<ProductCategory>[] = [
    {
        header: "Id",
        accessorKey: "categoryId",
        cell: ({ row }) => {
            const id: ProductCategory["categoryId"] = row.getValue("categoryId");
            return (
                <Link to={`/products/categories/${id}`}>
                    {id}
                </Link>
            );
        }
    },
    {
        header: "Name",
        accessorKey: "categoryName"
    }
];

export const ProductCategoriesList: React.FC = () => {
    const { data: categories, isLoading, error } = useProductCategories();

    if (isLoading) {
        return <div className="p-6">Loading products...</div>;
    }

    if (error) {
        return <div className="p-6 text-red-500">Error: {error.message}</div>;
    }

    if (!categories?.length) {
        return (
            <div className="p-6">
                <p className="mb-4">No product categories found</p>
            </div>
        );
    }

    return (
        <DataTable columns={columns} data={categories} />
    );
};