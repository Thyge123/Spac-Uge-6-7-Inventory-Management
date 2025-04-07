import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarGroup,
    SidebarGroupContent,
    SidebarGroupLabel,
    SidebarHeader,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from "@/components/ui/sidebar";
import type { SidebarGroupArgs } from '@/types';
import { Link } from 'react-router-dom';

export function AppSidebar() {

    const groups: SidebarGroupArgs[] = [
        {
            label: "Products",
            items: [
                {
                    label: "All products",
                    url: "/products"
                },
                {
                    label: "Product categories",
                    url: "/products/categories"
                }
            ]
        },
        {
            label: "Orders",
        },
        {
            label: "Customers",
        }
    ];

    return (
        <Sidebar>
            <SidebarHeader>Dashboard</SidebarHeader>
            <SidebarContent>
                {groups.map(({ label, items }) => (
                    <SidebarGroup key={`sidebarGroup-${label}`}>
                        <SidebarGroupLabel>
                            {label}
                        </SidebarGroupLabel>
                        <SidebarGroupContent>
                            <SidebarMenu>
                                {items ?
                                    items.map((item) => (
                                        <SidebarMenuItem key={label + item.label}>
                                            <SidebarMenuButton asChild>
                                                <Link to={item.url}>
                                                    {item.label}
                                                </Link>
                                            </SidebarMenuButton>
                                        </SidebarMenuItem>
                                    ))
                                    : null}
                            </SidebarMenu>
                        </SidebarGroupContent>
                    </SidebarGroup>
                ))}
            </SidebarContent>
            <SidebarFooter />
        </Sidebar >
    );
}