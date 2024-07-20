"use client"
import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
import Image from "next/image";
import { Metadata } from "next";
import DefaultLayout from "@/components/Layouts/DefaultLayout";
import NavBar from "@/components/DashboardNav";
import { Dialog, Transition } from '@headlessui/react';
import { Fragment, useEffect, useRef } from 'react'
import { ChangeEvent, FormEvent, useState } from "react";
import { CheckCircleIcon, ExclamationTriangleIcon, CodeBracketIcon } from '@heroicons/react/24/outline'
import { getCookie, setCookie, deleteCookie, hasCookie } from 'cookies-next';
import { jwtDecode, JwtPayload } from "jwt-decode";
import axiosInstance from "@/helpers/axiosInstance";
import Link from "next/link";

interface Music {
    id: string;
    musicTitle: string;
    musicUrl: string;
    musicPicture: string;
    musicPlays: number;
    musicDuration: number;
    releaseDate: string;
    genreName: string;
    artistName: string | null;
    albumDTO: AlbumDTO | null;
}

interface AlbumDTO {
    id: string;
    albumTitle: string;
    albumDescription: string;
    releaseDate: string;
    imageUrl: string | null;
}

interface Album {
    id: string;
    albumTitle: string;
    albumDescription: string;
    releaseDate: string;
    artist: {
        id: string;
        artistName: string;
    };
    musics: {
        $id: string;
        $values: Music[];
    };
    albumPicture: string;
}

function AlbumDetail({ params }: { params: { albumId: string } }) {
    const [album, setAlbum] = useState<Album | null>(null);
    const cancelButtonRef = useRef(null)

    const [musicId, setMusicId] = useState<string>('');
    const [open, setOpen] = useState(false)
    const [alerts, setAlert] = useState(false);
    const [openAlbum, setOpenAlbum] = useState(false)
    const [alertsAlbum, setAlertAlbum] = useState(false);
    const [audio, setAudio] = useState<HTMLAudioElement | null>(null);
    const [isPlaying, setIsPlaying] = useState(false);
    const [currentTime, setCurrentTime] = useState(0);
    const [duration, setDuration] = useState(0);
    const [title, setTitle] = useState<string>('');
    const [source, setSource] = useState<string>('');
    useEffect(() => {
        const fetchAlbum = async () => {
            try {
                const response = await axiosInstance.get<Album>(`/music-service/api/Album/getAlbumDetail/${params.albumId}`);
                setAlbum(response.data);
            } catch (error) {
                console.error('Error fetching album:', error);
            }
        };

        if (params.albumId) {
            fetchAlbum();
        }
    }, [params.albumId]);

    useEffect(() => {

        const newAudio = new Audio(source);
        setAudio(newAudio);

        return () => {
            if (audio) {
                audio.pause();
                audio.src = '';
                setAudio(null);
            }
        };
    }, [source]);

    useEffect(() => {
        if (!audio) return;

        const updateCurrentTime = () => {
            setCurrentTime(audio.currentTime);
        };

        const updateDuration = () => {
            setDuration(audio.duration);
        };

        audio.addEventListener('timeupdate', updateCurrentTime);
        audio.addEventListener('durationchange', updateDuration);

        return () => {
            audio.removeEventListener('timeupdate', updateCurrentTime);
            audio.removeEventListener('durationchange', updateDuration);
        };
    }, [audio]);

    const togglePlay = () => {
        if (!audio) return;
        if (isPlaying) {
            audio.pause();
        } else {
            audio.play();
        }
        setIsPlaying(!isPlaying);
    };

    const formatTime = (time: number): string => {
        const minutes = Math.floor(time / 60);
        const seconds = Math.floor(time % 60);
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    };

    const handleTimeSeek = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (!audio) return;
        const time = Number(e.target.value);
        audio.currentTime = time;
        setCurrentTime(time);
    };
    const skipForward = () => {
        if (!audio) return;
        audio.currentTime += 10;
        setCurrentTime(audio.currentTime);
    };

    const skipBackward = () => {
        if (!audio) return;
        audio.currentTime -= 10;
        setCurrentTime(audio.currentTime);
    };

    const handleDelete = async () => {

        try {
            const response = await axiosInstance.delete(`/music-service/api/Music/deleteMusic/${musicId}`);
            console.log(response);

            if (response.status === 200) {
                setOpen(false)
                setAlert(true);
            } else {
                setOpen(false)
                alert('Failed to upload file');
            }
        } catch (error) {
            console.error('Error uploading file:', error);
        } finally {
            console.log("end");
        }
    };
    const handleDeleteAlbum = async () => {

        try {
            const response = await axiosInstance.delete(`/music-service/api/Album/deleteAlbum/${params.albumId}`);
            console.log(response);

            if (response.status === 200) {
                setOpenAlbum(false)
                setAlertAlbum(true);
            } else {
                setOpenAlbum(false)
                alert('Failed to upload file');
            }
        } catch (error) {
            console.error('Error uploading file:', error);
        } finally {
            console.log("end");
        }
    };
    if (!album) {
        return <div>Loading...</div>;
    }
    return (
        <div className="border-t border-slate-200 lg:relative lg:mb-28 lg:ml-112 lg:border-t-0 xl:ml-120">
            <NavBar />
            <svg
                aria-hidden="true" className="absolute left-0 top-0 h-20 w-full">
                <defs>
                    <linearGradient id=":S1:-fade" x1="0" x2="0" y1="0" y2="1">
                        <stop offset="40%" stop-color="white"></stop>
                        <stop offset="100%" stop-color="black"></stop>
                    </linearGradient>
                    <linearGradient id=":S1:-gradient">
                        <stop offset="0%" stop-color="#4989E8"></stop>
                        <stop offset="50%" stop-color="#6159DA"></stop>
                        <stop offset="100%" stop-color="#FF54AD"></stop>
                    </linearGradient>
                    <mask id=":S1:-mask">
                        <rect width="100%" height="100%" fill="url(#:S1:-pattern)"></rect>
                    </mask>

                    <pattern id=":S1:-pattern" width="400" height="100%" patternUnits="userSpaceOnUse">
                        <rect width="2" height="83%" x="2" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="99%" x="6" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="52%" x="10" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="99%" x="14" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="86%" x="18" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="91%" x="22" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="92%" x="26" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="75%" x="30" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="51%" x="34" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="88%" x="38" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="45%" x="42" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="56%" x="46" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="80%" x="50" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="44%" x="54" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="93%" x="58" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="98%" x="62" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="41%" x="66" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="47%" x="70" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="87%" x="74" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="67%" x="78" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="73%" x="82" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="69%" x="86" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="88%" x="90" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="53%" x="94" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="69%" x="98" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="75%" x="102" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="86%" x="106" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="43%" x="110" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="80%" x="114" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="81%" x="118" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="78%" x="122" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="56%" x="126" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="47%" x="130" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="90%" x="134" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="50%" x="138" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="52%" x="142" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="77%" x="146" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="81%" x="150" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="98%" x="154" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="48%" x="158" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="87%" x="162" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="87%" x="166" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="55%" x="170" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="41%" x="174" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="42%" x="178" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="93%" x="182" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="84%" x="186" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="67%" x="190" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="68%" x="194" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="55%" x="198" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="57%" x="202" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="56%" x="206" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="55%" x="210" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="46%" x="214" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="67%" x="218" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="69%" x="222" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="79%" x="226" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="84%" x="230" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="63%" x="234" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="94%" x="238" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="90%" x="242" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="51%" x="246" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="73%" x="250" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="55%" x="254" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="57%" x="258" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="69%" x="262" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="88%" x="266" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="84%" x="270" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="49%" x="274" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="95%" x="278" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="73%" x="282" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="54%" x="286" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="57%" x="290" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="72%" x="294" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="51%" x="298" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="44%" x="302" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="52%" x="306" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="87%" x="310" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="93%" x="314" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="46%" x="318" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="47%" x="322" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="57%" x="326" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="79%" x="330" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="95%" x="334" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="54%" x="338" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="65%" x="342" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="90%" x="346" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="99%" x="350" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="82%" x="354" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="98%" x="358" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="93%" x="362" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="60%" x="366" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="50%" x="370" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="69%" x="374" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="77%" x="378" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="93%" x="382" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="44%" x="386" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="51%" x="390" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="96%" x="394" fill="url(#:S1:-fade)"></rect>
                        <rect width="2" height="60%" x="398" fill="url(#:S1:-fade)"></rect>
                    </pattern>

                </defs>
                <rect width="100%" height="100%" fill="url(#:S1:-gradient)" mask="url(#:S1:-mask)" opacity="0.25">
                </rect>
            </svg>
            <div className="relative">
                <article className="py-16 lg:py-36">
                    <div className="lg:px-8">
                        <div className="lg:max-w-4xl">
                            <div className="mx-auto px-4 sm:px-6 md:max-w-2xl md:px-4 lg:px-0">
                                <header className="flex flex-col">
                                    <div className="flex items-center gap-6"><button type="button"
                                        aria-label="Play episode 5: Bill Lumbergh"
                                        className="group relative flex h-18 w-18 flex-shrink-0 items-center justify-center rounded-full bg-slate-700 hover:bg-slate-900 focus:outline-none focus:ring focus:ring-slate-700 focus:ring-offset-4"><svg
                                            viewBox="0 0 36 36" aria-hidden="true"
                                            className="h-9 w-9 fill-white group-active:fill-white/80">
                                            <path
                                                d="M33.75 16.701C34.75 17.2783 34.75 18.7217 33.75 19.299L11.25 32.2894C10.25 32.8668 9 32.1451 9 30.9904L9 5.00962C9 3.85491 10.25 3.13323 11.25 3.71058L33.75 16.701Z">
                                            </path>
                                        </svg></button>
                                        <div className="flex flex-col">
                                            <h1 className="mt-2 text-4xl font-bold text-slate-900">{album.albumTitle}</h1><time
                                                dateTime="2022-02-24T00:00:00.000Z"
                                                className="order-first font-mono text-sm leading-7 text-slate-500">February 24,
                                                2022</time>
                                        </div>

                                    </div>
                                    <p className="ml-24 mt-3 text-lg font-medium leading-8 text-slate-700">{album.albumDescription}</p>
                                    <div className="ml-24 mt-6 flex items-center gap-4"><button type="button"
                                        onClick={() => {
                                            // setTitle(music.musicTitle)
                                            // setSource(music.musicUrl)
                                        }}
                                        aria-label="Play episode 5: Bill Lumbergh"
                                        className="flex items-center gap-x-3 text-sm font-bold leading-6 text-pink-500 hover:text-pink-700 active:text-pink-900"><svg
                                            aria-hidden="true" viewBox="0 0 10 10"
                                            className="h-2.5 w-2.5 fill-current">
                                            <path
                                                d="M8.25 4.567a.5.5 0 0 1 0 .866l-7.5 4.33A.5.5 0 0 1 0 9.33V.67A.5.5 0 0 1 .75.237l7.5 4.33Z">
                                            </path>
                                        </svg><span aria-hidden="true">Update</span></button><span
                                            aria-hidden="true"
                                            className="text-sm font-bold text-slate-400">/</span><button type="button"
                                                onClick={() => {
                                                    // setMusicId(music.id)
                                                     setOpenAlbum(true)
                                                }}
                                                className="flex items-center text-sm font-bold leading-6 text-pink-500 hover:text-pink-700 active:text-pink-900"
                                                aria-label="Show notes for episode 5: Bill Lumbergh">Delete album</button></div>

                                </header>
                                <hr className="my-12 border-gray-200" />
                                <div
                                    className="sm:pb-4">
                                    <div className="lg:px-8">
                                        <div className="lg:max-w-4xl">
                                            <div className="mx-auto px-4 sm:px-6 md:max-w-2xl md:px-4 lg:px-0">
                                                <h1 className="text-2xl font-bold leading-7 text-slate-900">Songs</h1>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="divide-y divide-slate-100 sm:mt-4 lg:mt-8 lg:border-t lg:border-slate-100">
                                        {album.musics.$values.map((music) => (
                                            <article key={music.id} aria-labelledby="episode-5-title" className="sm:py-6">
                                                <div className="lg:px-8">
                                                    <div className="lg:max-w-4xl">
                                                        <div className="mx-auto px-4 sm:px-6 md:max-w-2xl md:px-4 lg:px-0 mb-4 flex items-center gap-10">
                                                            <div className="h-14 w-14 rounded-full">
                                                                <img
                                                                    src={music.musicPicture} alt={music.musicTitle}
                                                                    width={55}
                                                                    height={55}
                                                                    className="rounded-full"
                                                                />
                                                            </div>
                                                            <div className="flex flex-col items-start">

                                                                <h2 id="episode-5-title" className="mt-2 text-lg font-bold text-slate-900"><a
                                                                    href="/5">{music.musicTitle}</a></h2><time
                                                                        dateTime="2022-02-24T00:00:00.000Z"
                                                                        className="order-first font-mono text-sm leading-7 text-slate-500">February
                                                                    24, 2022</time>
                                                                <p className="mt-1 text-base leading-7 text-slate-700"><strong>Artist: </strong>{music.artistName}</p>
                                                                <p className="mt-1 text-base leading-7 text-slate-700"><strong>Genre: </strong>{music.genreName}</p>
                                                                <div className="mt-4 flex items-center gap-4"><button type="button"
                                                                    onClick={() => {
                                                                        setTitle(music.musicTitle)
                                                                        setSource(music.musicUrl)
                                                                    }}
                                                                    aria-label="Play episode 5: Bill Lumbergh"
                                                                    className="flex items-center gap-x-3 text-sm font-bold leading-6 text-pink-500 hover:text-pink-700 active:text-pink-900"><svg
                                                                        aria-hidden="true" viewBox="0 0 10 10"
                                                                        className="h-2.5 w-2.5 fill-current">
                                                                        <path
                                                                            d="M8.25 4.567a.5.5 0 0 1 0 .866l-7.5 4.33A.5.5 0 0 1 0 9.33V.67A.5.5 0 0 1 .75.237l7.5 4.33Z">
                                                                        </path>
                                                                    </svg><span aria-hidden="true">Listen</span></button><span
                                                                        aria-hidden="true"
                                                                        className="text-sm font-bold text-slate-400">/</span><button type="button"
                                                                            onClick={() => {
                                                                                setMusicId(music.id)
                                                                                setOpen(true)
                                                                            }}
                                                                            className="flex items-center text-sm font-bold leading-6 text-pink-500 hover:text-pink-700 active:text-pink-900"
                                                                            aria-label="Show notes for episode 5: Bill Lumbergh">Delete song</button></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </article>
                                        ))}
                                    </div>
                                    {/* <h2 id="sponsors">Sponsors</h2>
                                    <ul>
                                        <li><a href="#">Initech</a> — Pioneers of the TPS report, Initech is actively looking for
                                            job-seekers with people skills who can work with customers to gather specifications and
                                            deliver them to the software people.</li>
                                        <li><a href="#">Globex Corporation</a> — Just a friendly and innocent high-tech company,
                                            with a casual work environment and an office without walls. Anything you’ve heard about
                                            a “doomsday device” is pure conjecture and not based in fact.</li>
                                    </ul>
                                    <h2 id="links">Links</h2>
                                    <ul>
                                        <li>Bill Lumbergh’s <a href="#">Twitter profile</a></li>
                                        <li>Bill Lumbergh’s <a href="#">personal website</a></li>
                                        <li>“What’s happening?”, Bill’s new book on effective management <a href="#">on Amazon</a>
                                        </li>
                                    </ul> */}
                                </div>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            <div className="fixed inset-x-0 bottom-0 z-10 lg:left-112 xl:left-120">
                <div
                    className="flex items-center gap-6 bg-white/90 px-4 py-4 shadow shadow-slate-200/80 ring-1 ring-slate-900/5 backdrop-blur-sm md:px-6">
                    <div className="hidden md:block"><button type="button"
                        onClick={togglePlay}
                        className="group relative flex h-10 w-10 flex-shrink-0 items-center justify-center rounded-full bg-slate-700 hover:bg-slate-900 focus:outline-none focus:ring-2 focus:ring-slate-700 focus:ring-offset-2 md:h-14 md:w-14"
                        aria-label="Play">
                        <div className="absolute -inset-3 md:hidden"></div><svg viewBox="0 0 36 36" aria-hidden="true"
                            className="h-5 w-5 fill-white group-active:fill-white/80 md:h-7 md:w-7">
                            <path
                                d="M33.75 16.701C34.75 17.2783 34.75 18.7217 33.75 19.299L11.25 32.2894C10.25 32.8668 9 32.1451 9 30.9904L9 5.00962C9 3.85491 10.25 3.13323 11.25 3.71058L33.75 16.701Z">
                            </path>
                        </svg>
                    </button></div>
                    <div className="mb-[env(safe-area-inset-bottom)] flex flex-1 flex-col gap-3 overflow-hidden p-1"><a
                        className="truncate text-center text-black text-sm font-bold leading-6 md:text-left" title="1: Skeletor"
                        href="/1">{title ? title : "Title"}</a>
                        <div className="flex justify-between gap-6">
                            <div className="flex items-center md:hidden"><button type="button"
                                onClick={skipBackward}
                                className="group relative rounded-md hover:bg-slate-100 focus:outline-none focus:ring-2 focus:ring-slate-400 focus:ring-offset-2 md:order-none"
                                aria-label="Mute">
                                <div className="absolute -inset-4 md:hidden"></div><svg aria-hidden="true"
                                    viewBox="0 0 24 24" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                                    className="h-6 w-6 fill-slate-500 stroke-slate-500 group-hover:fill-slate-700 group-hover:stroke-slate-700">
                                    <path
                                        d="M12 6L8 10H6C5.44772 10 5 10.4477 5 11V13C5 13.5523 5.44772 14 6 14H8L12 18V6Z">
                                    </path>
                                    <path d="M17 7C17 7 19 9 19 12C19 15 17 17 17 17" fill="none"></path>
                                    <path d="M15.5 10.5C15.5 10.5 16 10.9998 16 11.9999C16 13 15.5 13.5 15.5 13.5"
                                        fill="none"></path>
                                </svg>
                            </button></div>
                            <div className="flex flex-none items-center gap-4"><button type="button"
                                onClick={skipBackward}
                                className="group relative rounded-full focus:outline-none" aria-label="Rewind 10 seconds">
                                <div className="absolute -inset-4 -right-2 md:hidden"></div><svg aria-hidden="true"
                                    viewBox="0 0 24 24" fill="none" stroke-width="1.5" stroke-linecap="round"
                                    stroke-linejoin="round"
                                    className="h-6 w-6 stroke-slate-500 group-hover:stroke-slate-700">
                                    <path
                                        d="M8 5L5 8M5 8L8 11M5 8H13.5C16.5376 8 19 10.4624 19 13.5C19 15.4826 18.148 17.2202 17 18.188">
                                    </path>
                                    <path d="M5 15V19"></path>
                                    <path
                                        d="M8 18V16C8 15.4477 8.44772 15 9 15H10C10.5523 15 11 15.4477 11 16V18C11 18.5523 10.5523 19 10 19H9C8.44772 19 8 18.5523 8 18Z">
                                    </path>
                                </svg>
                            </button>
                                <div className="md:hidden"><button type="button"
                                    onClick={skipForward}
                                    className="group relative flex h-10 w-10 flex-shrink-0 items-center justify-center rounded-full bg-slate-700 hover:bg-slate-900 focus:outline-none focus:ring-2 focus:ring-slate-700 focus:ring-offset-2 md:h-14 md:w-14"
                                    aria-label="Play">
                                    <div className="absolute -inset-3 md:hidden"></div><svg viewBox="0 0 36 36"
                                        aria-hidden="true"
                                        className="h-5 w-5 fill-white group-active:fill-white/80 md:h-7 md:w-7">
                                        <path
                                            d="M33.75 16.701C34.75 17.2783 34.75 18.7217 33.75 19.299L11.25 32.2894C10.25 32.8668 9 32.1451 9 30.9904L9 5.00962C9 3.85491 10.25 3.13323 11.25 3.71058L33.75 16.701Z">
                                        </path>
                                    </svg>
                                </button></div><button type="button"
                                    onClick={skipForward}
                                    className="group relative rounded-full focus:outline-none"
                                    aria-label="Fast-forward 10 seconds">
                                    <div className="absolute -inset-4 -left-2 md:hidden"></div><svg aria-hidden="true"
                                        viewBox="0 0 24 24" fill="none"
                                        className="h-6 w-6 stroke-slate-500 group-hover:stroke-slate-700">
                                        <path
                                            d="M16 5L19 8M19 8L16 11M19 8H10.5C7.46243 8 5 10.4624 5 13.5C5 15.4826 5.85204 17.2202 7 18.188"
                                            stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M13 15V19" stroke-width="1.5" stroke-linecap="round"
                                            stroke-linejoin="round"></path>
                                        <path
                                            d="M16 18V16C16 15.4477 16.4477 15 17 15H18C18.5523 15 19 15.4477 19 16V18C19 18.5523 18.5523 19 18 19H17C16.4477 19 16 18.5523 16 18Z"
                                            stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                    </svg>
                                </button>
                            </div>
                            <div role="group" id="react-aria8752326642-:r0:" aria-labelledby="react-aria8752326642-:r1:"
                                className="absolute inset-x-0 bottom-full flex flex-auto touch-none items-center gap-6 md:relative">
                                <label className="sr-only" id="react-aria8752326642-:r1:">Current time</label>
                                <div className="relative w-full md:rounded-full pt-2" style={{ position: 'relative', touchAction: 'none' }}>

                                    <input
                                        className="relative w-full md:rounded-full"
                                        style={{
                                            backgroundColor: "red",
                                        }}
                                        type="range"
                                        min={0}
                                        max={duration}
                                        value={currentTime}
                                        onChange={handleTimeSeek}
                                    />

                                </div>

                                <div className="hidden items-center gap-2 md:flex"><output htmlFor="react-aria8752326642-:r1:-0"
                                    aria-live="off"
                                    className="hidden rounded-md px-1 py-0.5 font-mono text-sm leading-6 md:block text-slate-500">{formatTime(currentTime)}</output><span
                                        className="text-sm leading-6 text-slate-300" aria-hidden="true">/</span><span
                                            className="hidden rounded-md px-1 py-0.5 font-mono text-sm leading-6 text-slate-500 md:block">{formatTime(duration)}</span>
                                </div>
                            </div>
                            <div className="flex items-center gap-4">
                                <div className="flex items-center"><button type="button"
                                    className="relative flex h-6 w-6 items-center justify-center rounded-md text-slate-500 hover:bg-slate-100 hover:text-slate-700 focus:outline-none focus:ring-2 focus:ring-slate-400 focus:ring-offset-2"
                                    aria-label="Playback rate">
                                    <div className="absolute -inset-4 md:hidden"></div><svg aria-hidden="true"
                                        viewBox="0 0 16 16" fill="none" stroke="white" stroke-width="1.5"
                                        stroke-linecap="round" stroke-linejoin="round" className="h-4 w-4">
                                        <path
                                            d="M13 1H3C1.89543 1 1 1.89543 1 3V13C1 14.1046 1.89543 15 3 15H13C14.1046 15 15 14.1046 15 13V3C15 1.89543 14.1046 1 13 1Z"
                                            fill="currentColor" stroke="currentColor" stroke-width="2"></path>
                                        <path d="M3.75 7.25L5.25 5.77539V11.25"></path>
                                        <path d="M8.75 7.75L11.25 10.25"></path>
                                        <path d="M11.25 7.75L8.75 10.25"></path>
                                    </svg>
                                </button></div>
                                <div className="hidden items-center md:flex"><button type="button"
                                    className="group relative rounded-md hover:bg-slate-100 focus:outline-none focus:ring-2 focus:ring-slate-400 focus:ring-offset-2 md:order-none"
                                    aria-label="Mute">
                                    <div className="absolute -inset-4 md:hidden"></div><svg aria-hidden="true"
                                        viewBox="0 0 24 24" stroke-width="2" stroke-linecap="round"
                                        stroke-linejoin="round"
                                        className="h-6 w-6 fill-slate-500 stroke-slate-500 group-hover:fill-slate-700 group-hover:stroke-slate-700">
                                        <path
                                            d="M12 6L8 10H6C5.44772 10 5 10.4477 5 11V13C5 13.5523 5.44772 14 6 14H8L12 18V6Z">
                                        </path>
                                        <path d="M17 7C17 7 19 9 19 12C19 15 17 17 17 17" fill="none"></path>
                                        <path d="M15.5 10.5C15.5 10.5 16 10.9998 16 11.9999C16 13 15.5 13.5 15.5 13.5"
                                            fill="none"></path>
                                    </svg>
                                </button></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <Transition.Root show={open} as={Fragment}>
                <Dialog as="div" className="relative z-10" initialFocus={cancelButtonRef} onClose={setOpen}>
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </Transition.Child>

                    <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                        <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                            <Transition.Child
                                as={Fragment}
                                enter="ease-out duration-300"
                                enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                                enterTo="opacity-100 translate-y-0 sm:scale-100"
                                leave="ease-in duration-200"
                                leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                                leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                            >
                                <Dialog.Panel className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                                    <div className="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                                        <div className="sm:flex sm:items-start">
                                            <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:h-10 sm:w-10">
                                                <ExclamationTriangleIcon className="h-6 w-6 text-red" aria-hidden="true" />
                                            </div>
                                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                                <Dialog.Title as="h3" className="text-black font-semibold leading-6 text-gray-900">
                                                    Are you sure you want to delete this song?
                                                </Dialog.Title>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                                        <button
                                            type="button"
                                            className="inline-flex w-full justify-center rounded-md lg:bg-purple-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-purple-500 sm:ml-3 sm:w-auto"
                                            onClick={handleDelete}
                                        >
                                            Delele
                                        </button>
                                        <button
                                            type="button"
                                            className="text-black mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:mt-0 sm:w-auto"
                                            onClick={() => setOpen(false)}
                                            ref={cancelButtonRef}
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                </Dialog.Panel>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition.Root>
            <Transition.Root show={alerts} as={Fragment}>
                <Dialog as="div" className="relative z-10" initialFocus={cancelButtonRef} onClose={setAlert}>
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </Transition.Child>

                    <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                        <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                            <Transition.Child
                                as={Fragment}
                                enter="ease-out duration-300"
                                enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                                enterTo="opacity-100 translate-y-0 sm:scale-100"
                                leave="ease-in duration-200"
                                leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                                leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                            >
                                <Dialog.Panel className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                                    <div className="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                                        <div className="sm:flex sm:items-start">
                                            <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-green-100 sm:mx-0 sm:h-10 sm:w-10">
                                                <CheckCircleIcon className="h-6 w-6 text-green-400" aria-hidden="true" />
                                            </div>
                                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                                <Dialog.Title as="h3" className="text-black font-semibold leading-6 text-gray-900">
                                                    Delete music successfully!
                                                </Dialog.Title>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                                        <button
                                            type="button"
                                            className="inline-flex w-full justify-center rounded-md lg:bg-purple-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-purple-500 sm:ml-3 sm:w-auto"
                                            onClick={() => {
                                                setAlert(false)
                                            }}
                                        >
                                            Close
                                        </button>
                                    </div>
                                </Dialog.Panel>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition.Root>
            <Transition.Root show={openAlbum} as={Fragment}>
                <Dialog as="div" className="relative z-10" initialFocus={cancelButtonRef} onClose={setOpenAlbum}>
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </Transition.Child>

                    <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                        <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                            <Transition.Child
                                as={Fragment}
                                enter="ease-out duration-300"
                                enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                                enterTo="opacity-100 translate-y-0 sm:scale-100"
                                leave="ease-in duration-200"
                                leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                                leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                            >
                                <Dialog.Panel className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                                    <div className="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                                        <div className="sm:flex sm:items-start">
                                            <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:h-10 sm:w-10">
                                                <ExclamationTriangleIcon className="h-6 w-6 text-red" aria-hidden="true" />
                                            </div>
                                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                                <Dialog.Title as="h3" className="text-black font-semibold leading-6 text-gray-900">
                                                    Are you sure you want to delete this album?
                                                </Dialog.Title>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                                        <button
                                            type="button"
                                            className="inline-flex w-full justify-center rounded-md lg:bg-purple-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-purple-500 sm:ml-3 sm:w-auto"
                                            onClick={handleDeleteAlbum}
                                        >
                                            Delele
                                        </button>
                                        <button
                                            type="button"
                                            className="text-black mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:mt-0 sm:w-auto"
                                            onClick={() => setOpenAlbum(false)}
                                            ref={cancelButtonRef}
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                </Dialog.Panel>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition.Root>
            <Transition.Root show={alertsAlbum} as={Fragment}>
                <Dialog as="div" className="relative z-10" initialFocus={cancelButtonRef} onClose={setAlertAlbum}>
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </Transition.Child>

                    <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                        <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                            <Transition.Child
                                as={Fragment}
                                enter="ease-out duration-300"
                                enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                                enterTo="opacity-100 translate-y-0 sm:scale-100"
                                leave="ease-in duration-200"
                                leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                                leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                            >
                                <Dialog.Panel className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                                    <div className="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                                        <div className="sm:flex sm:items-start">
                                            <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-green-100 sm:mx-0 sm:h-10 sm:w-10">
                                                <CheckCircleIcon className="h-6 w-6 text-green-400" aria-hidden="true" />
                                            </div>
                                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                                <Dialog.Title as="h3" className="text-black font-semibold leading-6 text-gray-900">
                                                    Delete album successfully!
                                                </Dialog.Title>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                                        <Link href={"/artist/album"}><button
                                            type="button"
                                            className="inline-flex w-full justify-center rounded-md lg:bg-purple-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-purple-500 sm:ml-3 sm:w-auto"
                                            onClick={() => {
                                                setAlert(false)
                                            }}
                                        >
                                            Close
                                        </button></Link>
                                    </div>
                                </Dialog.Panel>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition.Root>
        </div>
        // <div className='text-black'>
        //   <h1>{album.albumTitle}</h1>
        //   <p>{album.albumDescription}</p>
        //   <p>Artist: {album.artist.artistName}</p>
        //   <p>Release Date: {new Date(album.releaseDate).toLocaleDateString()}</p>
        //   <img src={album.albumPicture} alt={album.albumTitle} style={{ width: '200px', height: '200px' }} />
        // </div>
    );
};


export default AlbumDetail;
