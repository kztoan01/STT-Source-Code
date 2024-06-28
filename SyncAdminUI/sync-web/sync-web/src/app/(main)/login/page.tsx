"use client";
import logo from "../../../../public/logo.jpg"
import Link from 'next/link';
import { FormEvent } from 'react';
import axios from 'axios';
import Image from "next/image";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "next/navigation";


function Login() {
    const router = useRouter();

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const email = formData.get("email") as string;
        const password = formData.get("password") as string;

        try {
            const res = await axios.post(
                "https://localhost:7023/music-service/api/Account/login",
                { email, password },
                { withCredentials: true }
            );

            console.log(res.data);
            if (res.data.token) {
                const decodedToken = jwtDecode(res.data.token);
                console.log(decodedToken);
                const role = (decodedToken as any).role;
    
                if (role === 'Admin') {
                    router.push("/admin");
                } else if (role === 'User') {
                    router.push("/artist");
                }
                router.refresh();
            } else {
                alert("Login failed");
            }
        } catch (error) {
            console.error('Error:', error);
            alert("Login failed");
        }
    };
    return (
        <>
            <div className="bg-white flex min-h-full flex-col justify-center px-6 py-12 lg:px-8 pt-0 relative isolate px-6 lg:px-8">
                <div className="sm:mx-auto sm:w-full sm:max-w-sm"> <Image className="mx-auto h-72 w-auto"
                    src={logo} alt="ArtHub" />
                    <h2 className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900">Sign in to your account
                    </h2>
                </div>
                <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
                    <form className="space-y-6" action="#" method="POST" onSubmit={(e) => handleSubmit(e)}>
                        <div> <label htmlFor="email" className="block text-sm font-medium leading-6 text-gray-900">Email address</label>
                            <div className="mt-2"> <input id="email" name="email" type="email" autoComplete="email" required
                                className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-purple-600 sm:text-sm sm:leading-6 text-center" />
                            </div>
                        </div>
                        <div>
                            <div className="flex items-center justify-between"> <label htmlFor="password"
                                className="block text-sm font-medium leading-6 text-gray-900">Password</label>
                                <div className="text-sm"> <a href="#" className="font-semibold text-purple-600 hover:text-purple-500">Forgot
                                    password?</a> </div>
                            </div>
                            <div className="mt-2"> <input id="password" name="password" type="password" autoComplete="current-password"
                                required
                                className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-purple-600 sm:text-sm sm:leading-6 text-center" />
                            </div>
                        </div>
                        <div className="relative flex gap-x-3">
                            <div className="flex h-6 items-center"> <input id="remember" name="remember" type="checkbox"
                                className="h-4 w-4 rounded border-gray-300 text-purple-600 focus:ring-purple-600" /> </div>
                            <div className="text-sm leading-6">
                                {/* <label for="remember" className="font-medium text-gray-900">Remember me</label> */}
                                <p className="text-gray-500">Remember me</p>
                            </div>
                        </div>
                        <div> <button
                            type="submit"
                            className="flex w-full justify-center rounded-md lg:bg-purple-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-purple-500 hover:text-white focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-purple-600"
                        >
                            Log in
                        </button> </div>
                        <div className="flex flex-row justify-center items-center mt-6">
                            <hr className="border w-full" />
                            <p className="flex flex-shrink-0 px-4 text-base leading-4 text-gray-600">or</p>
                            <hr className="border w-full" />
                        </div>

                    </form>
                    <p className="mt-10 text-center text-sm text-gray-500"> Not a member? <Link href="/"
                        className="font-semibold leading-6 text-purple-600 hover:text-purple-500">Sign up Now</Link> </p>
                </div>
            </div>
        </>
    );
};

export default Login;