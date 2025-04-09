import { redirect } from 'react-router-dom';

export const userIsLoggedIn = () => {
    return Boolean(sessionStorage.getItem("authToken"));
};

export const authLoader = () => {
    console.log(userIsLoggedIn());
    if (!userIsLoggedIn()) {
        return redirect("/login");
    }
    return null;
};

export const loginLoader = () => {

    if (userIsLoggedIn()) {
        return redirect("/products");
    }
    return null;
};