import { Outlet, createBrowserRouter, redirect, RouterProvider } from 'react-router-dom';
import {
    QueryClientProvider,
} from '@tanstack/react-query';
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { queryClient } from '@/api/queryClient';
// import Navbar from '@/components/layout/Navbar';sss
import { Toaster } from 'sonner';
import { ProductList } from '@/pages/products/components/ProductsList';
import { ProductCategoriesList } from '@/pages/products.categories/components/ProductCategoriesList';
import { ProductCategoryDetail } from '@/pages/products.categories/components/ProductCategoryDetail';
import { ProductDetail } from '@/pages/products/components/ProductsDetail';
import { SidebarProvider } from '@/components/ui/sidebar';
import { DashboardSidebar } from '@/components/layout/DashboardSidebar';
import { DashboardNavbar } from '@/components/layout/DashboardNavbar';

const RootLayout: React.FC = () => (
    <SidebarProvider>
        <div className="min-h-screen min-w-screen flex flex-col">
            <header className="sticky top-0 z-50 w-full h-[72px] border-b bg-sidebar px-4 backdrop-blur supports-[backdrop-filter]:bg-sidebar/60">
                <div className="container flex h-full items-center">
                    <p className='text-lg px-4'>
                        Admin Dashboard
                    </p>
                    <DashboardNavbar />
                </div>
            </header>
            <div className="flex flex-1">
                <DashboardSidebar />
                <main className="flex-1 p-8 bg-white min-h-[calc(100vh-64px)]">
                    <Outlet />
                </main>
            </div>
            <Toaster position="top-center" />
        </div>
    </SidebarProvider>
);

// Data router configuration
const router = createBrowserRouter([
    {
        path: "/",
        Component: RootLayout,
        children: [
            // Default redirect to /products
            {
                index: true,
                loader: async () => redirect("/products"),
            },
            // Product routes
            { path: "products", Component: ProductList },
            { path: "products/:id", Component: ProductDetail },
            { path: "products/categories", Component: ProductCategoriesList },
            { path: "products/categories/:id", Component: ProductCategoryDetail }
            // { path: "products/:id", Component: ProductDetail },
            // // Order routes
            // { path: "orders", Component: OrderList },
            // { path: "orders/:id", Component: OrderDetail },
            // // User routes
            // { path: "users", Component: UserList },
            // { path: "users/:id", Component: UserDetail },
        ],
    },
]);

export default function App() {
    return (
        <QueryClientProvider client={queryClient}>
            {/* React Query Devtools */}
            <RouterProvider router={router} />
            {import.meta.env.VITE_ENABLE_QUERY_DEVTOOLS === "true" && (
                <ReactQueryDevtools initialIsOpen={false} />
            )}
            {/* <Router>
                <nav className="p-4 bg-blue-600 text-white flex gap-4">
                    <Link to="/products">Products</Link>
                </nav>
                <Routes>
                    <Route path="/products" element={<Products />} />
                </Routes>
            </Router> */}
        </QueryClientProvider>
    );
}
