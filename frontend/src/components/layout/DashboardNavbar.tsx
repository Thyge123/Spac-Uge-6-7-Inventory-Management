import {
    NavigationMenu,
    NavigationMenuItem,
    NavigationMenuList,
} from "@/components/ui/navigation-menu";
import type React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import clsx from 'clsx';
import { userIsLoggedIn } from '@/pages/auth/utils/loaders';
import AuthService from '@/pages/auth/services/AuthService';

const baseLinks = [
    {
        title: "Home",
        href: "/",
    },

];

export const DashboardNavbar: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();

    const logout = () => {
        AuthService.logout();
        navigate(0);
    };

    return (
        <NavigationMenu>
            <NavigationMenuList>
                {baseLinks.map((link) => (
                    <NavigationMenuItem key={`navlink-${link.title}`}>
                        <Link to={link.href} className={
                            clsx(
                                "group inline-flex h-9 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground",
                                location.pathname !== link.href && "bg-sidebar",
                                location.pathname === link.href && "bg-accent text-accent-foreground",
                            )
                        }>
                            {link.title}
                        </Link>
                    </NavigationMenuItem>
                ))}
            </NavigationMenuList>
            <NavigationMenuList>
                <NavigationMenuItem>
                    {userIsLoggedIn()
                        ? <a onClick={() => logout()}>
                            Logout
                        </a>
                        : null
                    }
                </NavigationMenuItem>
            </NavigationMenuList>
        </NavigationMenu>
    );
};