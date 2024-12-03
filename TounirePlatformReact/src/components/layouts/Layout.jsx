import { Outlet } from "react-router-dom"
import NavBar from "./NavBar"
import { ToastContainer } from "react-toastify"

const Layout = () => {

    return (
        <>
        <header>
            <NavBar />
        </header>
        <main> 
            <Outlet />
        </main>
        <ToastContainer/>
        </>
    )
}


export default Layout