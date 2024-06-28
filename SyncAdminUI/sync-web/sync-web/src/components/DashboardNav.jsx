"use client"

import Link from 'next/link'
import Image from 'next/image'
import logo from "../../public/logo.jpg"
import { usePathname } from 'next/navigation'

export default function NavBar() {
    const pathName = usePathname();

    const siteRoute = [
        {
            href: '/artist',
            name: 'Musics'
        },
        {
            href: '/artist/upload',
            name: 'Upload'
        },
        {
            href: '/artist/statistic',
            name: 'Statistic'
        },
        {
            href: '/artist/account',
            name: 'Account'
        }

    ]

    return (
        <header className="pointer-events-none relative z-50 flex flex-none flex-col">
            <div className="top-0 z-10 h-16 pt-6">
                <div className="sm:px-8 top-[var(--header-top,theme(spacing.6))] w-full">
                    <div className="mx-auto w-full max-w-7xl lg:px-8">
                        <div className="relative px-4 sm:px-8 lg:px-12">
                            <div className="mx-auto max-w-2xl lg:max-w-5xl">
                                <div className="relative flex gap-4">
                                    <div className='flex flex-1 justify-end md:justify-center'>
                                        <nav className='pointer-events-auto hidden md:block'>
                                            <ul className='flex rounded-full bg-white/90 px-3 text-sm font-medium text-zinc-800 shadow-lg shadow-zinc-800/5 ring-1 ring-zinc-900/5 backdrop-blur dark:bg-zinc-200/90 dark:text-zinc-800 dark:ring-white/10'>
                                                {siteRoute.map((site) => (
                                                    <li key={site.href}>
                                                        <Link href={site.href} className={`cursor-pointer relative block px-3 py-2 transition ${pathName === site.href ? "text-purple-900 dark:text-purple-800" : "hover:text-purple-800 dark:hover:text-purple-800"} `}>
                                                            {site.name}
                                                            {pathName === site.href ? <span className='absolute inset-x-1 -bottom-px h-px bg-gradient-to-r from-teal-500/0 via-purple-500/40 to-teal-500/0 dark:from-teal-400/0 dark:via-purple-400/40 dark:to-teal-400/0'></span> : <></>}
                                                        </Link>
                                                    </li>
                                                ))}
                                            </ul>
                                        </nav>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </header>

    )
}