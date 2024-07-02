"use client"
import { Fragment, useState } from 'react'
import { Dialog, Disclosure, Popover, Transition, Menu } from '@headlessui/react'
import {
    Bars3Icon,
    ChartPieIcon,
    XMarkIcon,
    HeartIcon,
    PaintBrushIcon,
    BugAntIcon,
    BoltIcon,
    PhotoIcon,
    PencilIcon,
    SparklesIcon,
    CubeTransparentIcon,
    MagnifyingGlassIcon,
    ShoppingBagIcon,
} from '@heroicons/react/24/outline'
import { ChevronDownIcon, PhoneIcon, PlayCircleIcon } from '@heroicons/react/20/solid'
import { useEffect, useContext } from 'react'
import axios from 'axios'
import logo from "../../public/logo.jpg"
import Image from "next/image";
import Link from 'next/link'
const products = [
    { name: 'Caricature', state: 'Caricature', description: 'A drawing that makes someone look funny', href: '#', icon: PaintBrushIcon },
    { name: 'Cartoon', state: 'Cartoon', description: 'Cartoon drawings depict a more comedic view. ', href: '#', icon: BugAntIcon },
    { name: 'Figure', state: 'Figure', description: 'Artists create figure drawings', href: '#', icon: PhotoIcon },
    { name: 'Gesture', state: 'Gesture', description: 'Gesture drawing uses real-life subjects', href: '#', icon: SparklesIcon },
    { name: 'Photorealism', state: 'Photorealism', description: 'Photorealism, also called hyperrealism', href: '#', icon: PencilIcon },
    { name: 'Scientific illustrations', state: 'Scientific illustrations', description: 'Pointillism is an artistic technique', href: '#', icon: BoltIcon },
    { name: 'Sketch', state: 'Sketch', description: 'Communicate complex concepts in an easy-to-comprehend way', href: '#', icon: ChartPieIcon },
    { name: 'Technical', state: 'Technical', description: 'Technical drawing is the creation of precise diagrams', href: '#', icon: CubeTransparentIcon },
]

const navigation = [
    { name: 'Dashboard', href: '#', current: true, link: '/instructordashboard/dashboard' },
    { name: 'Courses', href: '#', current: false, link: '/instructordashboard/courses' },
    { name: 'Student', href: '#', current: false, link: '/instructordashboard/student' },
    { name: 'Reports', href: '#', current: false, link: '/instructordashboard/reports' },
    { name: 'Performance', href: '#', current: false, link: '/instructordashboard/performance' },
]
const handleLogout = () => {
    localStorage.removeItem("STU-authenticated");
    localStorage.removeItem("AD-authenticated");
    localStorage.removeItem("logined");
}


function classNames(...classes: string[]) {
    return classes.filter(Boolean).join(' ')
}

interface User {
    lastname: string;
    firstname: string;
    email: string;
    image?: string;
}

export default function Nav(props: { login: string; signup: string }) {

    const [banner, setBanner] = useState(true)
    const [users, setUsers] = useState()
    const getUsers = async () => {
        try {
            const response = await axios.get("/accounts");
            setUsers(response.data);
        } catch (err) {
            console.log(err);
        }
    }

    useEffect(() => {
        getUsers();
    }, []
    )
    const [open, setOpen] = useState(false)
    let logStatus = '';
    let sigStatus = '';
    let userNavigation: any[] = []
    let user: any[] = []
    let login = false
    useEffect(() => {
        // Check if running in the browser
        if (typeof window !== 'undefined') {
            const loginedUserString = localStorage.getItem("logined");
            const loginedUser: User | null = loginedUserString ? JSON.parse(loginedUserString) : null;
            if (localStorage.getItem("logined")) {
                login = true
                userNavigation = [
                    { name: loginedUser?.lastname + " " + loginedUser?.firstname, href: '#', link: '/account/setting' },
                    { name: loginedUser?.email, href: '#', link: '/account/setting' },
                    { name: 'Cart', href: '#', link: '/error' },
                    { name: 'Wishlist Cart', href: '#', link: '/error' },
                    { name: 'My Learning', href: '#', link: '/account/learning' },
                    { name: 'Notifications', href: '#', link: '/account/notification' },
                    { name: 'Account Settings', href: '#', link: '/account/setting' },
                    { name: 'Purchase history', href: '#', link: '/account/purchase' },
                ]
                user = [{
                    name: loginedUser?.lastname + " " + loginedUser?.firstname,
                    email: loginedUser?.email,
                    imageUrl:
                        linkImg + loginedUser?.image,
                }]
            } else {
                logStatus = props.login
                sigStatus = props.signup
            }
        }
    }, []);
    const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
    const linkImg = 'https://storage.cloud.google.com/Sync-bucket/'


    return (
        <>
            {banner ? (
                <div className="relative isolate flex items-center gap-x-6 overflow-hidden bg-gray-50 px-6 py-2.5 sm:px-3.5 sm:before:flex-1">
                    <div className="absolute left-[max(-7rem,calc(50%-52rem))] top-1/2 -z-10 -translate-y-1/2 transform-gpu blur-2xl" aria-hidden="true">
                        <div className="aspect-[577/310] w-[36.0625rem] bg-gradient-to-r from-[#ff80b5] to-[#9089fc] opacity-30"></div>
                    </div>
                    <div className="absolute left-[max(45rem,calc(50%+8rem))] top-1/2 -z-10 -translate-y-1/2 transform-gpu blur-2xl" aria-hidden="true">
                        <div className="aspect-[577/310] w-[36.0625rem] bg-gradient-to-r from-[#ff80b5] to-[#9089fc] opacity-30"></div>
                    </div>
                    <div className="flex flex-wrap items-center gap-x-4 gap-y-2">
                        <p className="text-sm leading-6 text-gray-900">
                            <strong className="font-semibold">Sync 2023</strong>
                            <svg viewBox="0 0 2 2" className="mx-2 inline h-0.5 w-0.5 fill-current" aria-hidden="true">
                                <circle cx="1" cy="1" r="1" />
                            </svg>
                            Register for the course from August 22 - December 22 to receive the best deals.
                        </p>
                        <Link href="/login" className="flex-none rounded-full bg-gray-900 px-3.5 py-1 text-sm font-semibold text-white shadow-sm hover:bg-gray-700 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-gray-900">
                            Register now <span aria-hidden="true">&rarr;</span>
                        </Link>
                    </div>
                    <div className="flex flex-1 justify-end">
                        <button onClick={() => setBanner(false)} type="button" className="-m-3 p-3 focus-visible:outline-offset-[-4px]">
                            <span className="sr-only">Dismiss</span>
                            <svg className="h-5 w-5 text-gray-900" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                                <path d="M6.28 5.22a.75.75 0 00-1.06 1.06L8.94 10l-3.72 3.72a.75.75 0 101.06 1.06L10 11.06l3.72 3.72a.75.75 0 101.06-1.06L11.06 10l3.72-3.72a.75.75 0 00-1.06-1.06L10 8.94 6.28 5.22z" />
                            </svg>
                        </button>
                    </div>
                </div>
            ) : null}

            <header className="bg-white">
                <nav className="mx-auto flex max-w-7xl items-center justify-between p-6 lg:px-8" aria-label="Global">
                    <div className="flex lg:flex-1">
                        <Link href="/" className="-m-1.5 p-1.5">
                            <span className="sr-only">Sync</span>
                            <Image className="h-10 w-auto" src={logo} alt="" />
                        </Link>
                    </div>
                    <div className="flex lg:hidden">
                        <button type="button" className="-m-2.5 inline-flex items-center justify-center rounded-md p-2.5 text-gray-700 outline-0" onClick={() => setMobileMenuOpen(true)}>
                            <span className="sr-only">Open main menu</span>
                            <Bars3Icon className="h-6 w-6" aria-hidden="true" />
                        </button>
                    </div>
                    <Popover.Group className="hidden lg:flex lg:gap-x-12">
                        <Popover className="relative">
                            <Popover.Button className="flex items-center gap-x-1 text-sm font-semibold leading-6 text-gray-900">
                                Categories
                                <ChevronDownIcon className="h-5 w-5 flex-none text-gray-400" aria-hidden="true" />
                            </Popover.Button>
                            <Transition as={Fragment} enter="transition ease-out duration-200" enterFrom="opacity-0 translate-y-1" enterTo="opacity-100 translate-y-0" leave="transition ease-in duration-150" leaveFrom="opacity-100 translate-y-0" leaveTo="opacity-0 translate-y-1">
                                <Popover.Panel className="absolute -left-8 top-full z-10 mt-3 w-screen max-w-md overflow-hidden rounded-3xl bg-white shadow-lg ring-1 ring-gray-900/5">
                                    <div className="p-4">
                                        {products?.map((item) => (
                                            <div key={item.name} className="group relative flex items-center gap-x-6 rounded-lg p-4 text-sm leading-6 hover:bg-gray-50">
                                                <div className="flex h-11 w-11 flex-none items-center justify-center rounded-lg bg-gray-50 group-hover:bg-white">
                                                    <item.icon className="h-6 w-6 text-gray-600 group-hover:text-purple-600" aria-hidden="true" />
                                                </div>
                                                <div className="flex-auto">
                                                    <Link href={item.href} className="block font-semibold text-gray-900">
                                                        {item.name}
                                                        <span className="absolute inset-0" />
                                                    </Link>
                                                    <p className="mt-1 text-gray-600">{item.description}</p>
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                </Popover.Panel>
                            </Transition>
                        </Popover>
                        <Link href="/instructordashboard" className="text-sm font-semibold leading-6 text-gray-900">
                            Instructor
                        </Link>
                        <Link href="/account/learning" className="text-sm font-semibold leading-6 text-gray-900">
                            My Learning
                        </Link>
                        <Link href="/search" className="text-sm font-semibold leading-6 text-gray-900">
                            Explore
                        </Link>
                        <Link href="#" className="text-sm font-semibold leading-6 text-gray-900">
                            Notifications
                        </Link>
                    </Popover.Group>
                    <div className="flex lg:ml-6">
                        <Link href="#" className="p-2 text-gray-400 hover:text-gray-500">
                            <span className="sr-only">Search</span>
                            <MagnifyingGlassIcon className="h-6 w-6" aria-hidden="true" />
                        </Link>
                    </div>
                    <div className="hidden lg:flex lg:flex-1 lg:justify-end">
                        <Link href="/login" className="text-sm font-semibold leading-6 text-gray-900 mr-5">
                            Login
                        </Link>
                        <Link href="/signup" className="text-sm font-semibold leading-6 text-gray-900">
                            {sigStatus}
                        </Link>
                    </div>
                    {login ? (
                        <div>
                            <Menu as="div" className="relative ml-3">
                                <div>
                                    <Menu.Button className="relative flex max-w-xs items-center rounded-full bg-gray-800 text-sm focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800">
                                        <span className="absolute -inset-1.5" />
                                        <span className="sr-only">Open user menu</span>
                                        <Image className="h-8 w-8 rounded-full object-cover object-center" src={logo} alt="" />
                                    </Menu.Button>
                                </div>
                                <Transition as={Fragment} enter="transition ease-out duration-100" enterFrom="transform opacity-0 scale-95" enterTo="transform opacity-100 scale-100" leave="transition ease-in duration-75" leaveFrom="transform opacity-100 scale-100" leaveTo="transform opacity-0 scale-95">
                                    <Menu.Items className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                                        {userNavigation.map((item) => (
                                            <Menu.Item key={item.name}>
                                                {({ active }) => (
                                                    <Link href={item.href} className={classNames(active ? 'bg-gray-100' : '', 'block px-4 py-2 text-sm text-gray-700')}>
                                                        {item.name}
                                                    </Link>
                                                )}
                                            </Menu.Item>
                                        ))}
                                        <Menu.Item>
                                            {({ active }) => (
                                                <button onClick={() => {
                                                    handleLogout()
                                                }} className={classNames(active ? 'bg-gray-100' : '', 'block px-4 py-2 text-sm text-gray-700')}>
                                                    Sign out
                                                </button>
                                            )}
                                        </Menu.Item>
                                    </Menu.Items>
                                </Transition>
                            </Menu>
                        </div>
                    ) : null}
                </nav>
                <Dialog as="div" className="lg:hidden" open={mobileMenuOpen} onClose={setMobileMenuOpen}>
                    <div className="fixed inset-0 z-10" />
                    <Dialog.Panel className="fixed inset-y-0 right-0 z-10 w-full overflow-y-auto bg-white px-6 py-6 sm:max-w-sm sm:ring-1 sm:ring-gray-900/10">
                        <div className="flex items-center justify-between">
                            <Link href="/" className="-m-1.5 p-1.5">
                                <span className="sr-only">Your Company</span>
                                <Image className="h-8 w-auto" src={logo} alt="" />
                            </Link>
                            <button type="button" className="-m-2.5 rounded-md p-2.5 text-gray-700 outline-0" onClick={() => setMobileMenuOpen(false)}>
                                <span className="sr-only">Close menu</span>
                                <XMarkIcon className="h-6 w-6" aria-hidden="true" />
                            </button>
                        </div>
                        <div className="mt-6 flow-root">
                            <div className="-my-6 divide-y divide-gray-500/10">
                                <div className="space-y-2 py-6">
                                    {products?.map((item) => (
                                        <Link key={item.name} href={item.href} className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                            {item.name}
                                        </Link>
                                    ))}
                                </div>
                                <div className="py-6">
                                    <Link href="/instructordashboard" className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                        Instructor
                                    </Link>
                                    <Link href="/account/learning" className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                        My Learning
                                    </Link>
                                    <Link href="/search" className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                        Explore
                                    </Link>
                                    <Link href="#" className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                        Notifications
                                    </Link>
                                </div>
                                <div className="py-6">
                                    <Link href="/login" className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                        {logStatus}
                                    </Link>
                                    <Link href="/signup" className="-mx-3 block rounded-lg py-2 px-3 text-base font-semibold leading-7 text-gray-900 hover:bg-gray-400/10">
                                        {sigStatus}
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </Dialog.Panel>
                </Dialog>
            </header>
        </>

    )
}