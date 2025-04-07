import { Outlet, createBrowserRouter, redirect, RouterProvider } from 'react-router-dom';
import {
    QueryClientProvider,
} from '@tanstack/react-query';
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { queryClient } from './api/queryClient';
import Navbar from './layout/Navbar';
import Sidebar from './layout/Sidebar';
import { Toaster } from 'sonner';
import ProductList from './pages/products/components/ProductsList';

const RootLayout: React.FC = () => (
    <div className="min-h-screen flex flex-col">
        <Navbar />
        <div className="flex flex-1">
            <Sidebar />
            <main className="flex-1 p-8 bg-white min-h-[calc(100vh-64px)]">
                <Outlet />
            </main>
        </div>
        <Toaster position="top-center" />
    </div>
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
