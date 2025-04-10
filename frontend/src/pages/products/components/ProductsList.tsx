import {
    useProducts,
} from "../queries/ProductQueries";
import {
    ColumnDef,
    type PaginationState,
} from "@tanstack/react-table";
import type { ProductCategory, Product } from '@/types';
import { DataTable } from '@/components/ui/data-table';
import { Link } from 'react-router-dom';
import { useState } from 'react';

const columns: ColumnDef<Product>[] = [
    {
        header: "Id",
        accessorKey: "productId",
        cell: ({ row }) => {
            const id: number = row.getValue("productId");
            return (
                <Link to={`/products/${id}`}>
                    {id}
                </Link>
            );
        }
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

            return <div className="font-medium">{formatted}</div>;
        },
    },
    {
        header: "Category",
        accessorKey: "category",
        cell: ({ row }) => {
            const { categoryId, categoryName }: ProductCategory = row.getValue("category");
            return (
                <Link to={`/products/categories/${categoryId}`}>
                    {categoryId + " | " + categoryName}
                </Link>
            );
        }
    },
    {
        header: "Quantity",
        accessorKey: "quantity"
    }
];

const ITEMS_PER_PAGE = 40;

export const ProductList: React.FC = () => {
    const [pagination, setPagination] = useState<PaginationState>({ pageIndex: 0, pageSize: ITEMS_PER_PAGE });
    // const ITEMS_PER_PAGE = 10;
    // const [showCreateForm, setShowCreateForm] = useState(false);
    // const [newProduct, setNewProduct] = useState<
    //     Omit<Product, "id" | "createdAt" | "updatedAt">
    // >({
    //     name: "",
    //     description: "",
    //     price: 0,
    //     stock: 0,
    //     category: "",
    // });

    const { data, isLoading, error } = useProducts({
        pageNumber: pagination.pageIndex + 1,
        pageSize: pagination.pageSize
    });



    // const deleteProduct = useDeleteProduct();
    // const createProduct = useCreateProduct();
    // const queryClient = useQueryClient();

    // const handleDelete = async (id: string, e: React.MouseEvent) => {
    //     e.stopPropagation(); // Prevent navigation when clicking delete
    //     // Get the current query data
    //     const previousData = queryClient.getQueryData([
    //         ...queryKeys.products.all,
    //         { page, limit: ITEMS_PER_PAGE },
    //     ]);
    //     try {
    //         // Optimistically update the UI by filtering out the deleted product
    //         queryClient.setQueryData(
    //             [...queryKeys.products.all, { page, limit: ITEMS_PER_PAGE }],
    //             (old: any) => {
    //                 return {
    //                     ...old,
    //                     data: old.data.filter((product: Product) => product.id !== id),
    //                 };
    //             }
    //         );

    //         // Perform the actual deletion
    //         await deleteProduct.mutateAsync(id);

    //         // If the page is now empty and it's not the first page, go to previous page
    //         if (data?.data.length === 1 && page > 1) {
    //             setPage(page - 1);
    //         }
    //     } catch (error) {
    //         console.error("Failed to delete product:", error);
    //         // If there was an error, restore the previous data
    //         queryClient.setQueryData(
    //             [...queryKeys.products.all, { page, limit: ITEMS_PER_PAGE }],
    //             previousData
    //         );
    //     }
    // };

    // const handleCreate = async (e: React.FormEvent) => {
    //     e.preventDefault();
    //     try {
    //         await createProduct.mutateAsync(newProduct);
    //         // Reset form and hide it after successful creation
    //         setNewProduct({
    //             name: "",
    //             description: "",
    //             price: 0,
    //             stock: 0,
    //             category: "",
    //         });
    //         setShowCreateForm(false);
    //     } catch (error) {
    //         console.error("Failed to create product:", error);
    //     }
    // };

    // const handleInputChange = (
    //     e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    // ) => {
    //     const { name, value } = e.target;
    //     setNewProduct((prev) => ({
    //         ...prev,
    //         [name]: name === "price" || name === "stock" ? Number(value) : value,
    //     }));
    // };

    console.log(data?.products);

    if (isLoading) {
        return <div className="p-6">Loading products...</div>;
    }

    if (error) {
        return <div className="p-6 text-red-500">Error: {error.message}</div>;
    }

    if (!data?.products?.length) {
        return (
            <div className="p-6">
                <p className="mb-4">No products found</p>
                {/* <button
                    onClick={() => setShowCreateForm(true)}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                >
                    Add New Product
                </button>
                {showCreateForm && (
                    <ProductForm
                        product={newProduct}
                        onSubmit={handleCreate}
                        onChange={handleInputChange}
                        onCancel={() => setShowCreateForm(false)}
                        isSubmitting={createProduct.isPending}
                        title="Add New Product"
                        submitLabel="Create Product"
                    />
                )} */}
            </div>
        );
    }

    return (
        <DataTable columns={columns} data={data.products} pagination={pagination} setPagination={setPagination} pageCount={data.totalPages} />
    );
};
