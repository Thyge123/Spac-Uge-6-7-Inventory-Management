import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Products from './pages/Products';
import {
    QueryClient,
    QueryClientProvider,
} from '@tanstack/react-query';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            gcTime: 1000 * 60 * 60 * 24, // 24 hours
        },
    },
});

export default function App() {
    return (
        <QueryClientProvider client={queryClient}>

            <Router>
                <nav className="p-4 bg-blue-600 text-white flex gap-4">
                    <Link to="/products">Products</Link>
                </nav>
                <Routes>
                    <Route path="/products" element={<Products />} />
                </Routes>
            </Router>
        </QueryClientProvider>
    );
}
