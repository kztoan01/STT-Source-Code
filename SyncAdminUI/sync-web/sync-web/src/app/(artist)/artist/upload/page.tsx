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
import useAlbums from "@/helpers/AlbumHook";
import Link from "next/link";

interface MyJwtPayload extends JwtPayload {
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
}

interface Genre {
    id: string;
    genreName: string;
    genreDescription: string;
}
type Artist = {
    id: string;
    userId: string;
    artistName: string;
    artistDescription: string;
    numberOfFollower: number;
  };
  
  type Album = {
    id: string;
    albumTitle: string;
    albumDescription: string;
    releaseDate: string;
    artist: Artist;
    albumPicture: string;
  };
  
  type ApiResponse = {
    $id: string;
    $values: Album[];
  };
const Upload = () => {
    

    const [image, setImage] = useState<File | null>(null);

    const [music, setMusic] = useState<File | null>(null);

    const [title, setTitle] = useState<string>('');

    const [genre, setGenre] = useState<string>('');

    const [album, setAlbum] = useState<string>('');

    const [message, setMessage] = useState<string>('');

    const [preview, setPreview] = useState<string | null>(null);

    const [loading, setLoading] = useState(false)
    const [alert, setAlert] = useState(false)
    const [albums, setAlbums] = useState<Album[]>([]);
    const cancelButtonRef = useRef(null)

    const [genres, setGenres] = useState<Genre[]>([]);

    const [selectedGenre, setSelectedGenre] = useState<string>('');

    const handleImage = (e: ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setImage(file);
            setPreview(URL.createObjectURL(file));
        }
    }
    const handleMusic = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setMusic(e.target.files[0]);
        }
    }

    const [artistId, setArtistId] = useState<string>('');
    useEffect(() => {
        const token = getCookie('token');
        console.log('Token:', token);

        if (typeof token === 'string') {
            try {
                const decoded = jwtDecode<MyJwtPayload>(token);
                console.log('Decoded Token:', decoded);

                const id = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
                setArtistId(id);
            } catch (error) {
                console.error('Failed to decode token:', error);
            }
        } else {
            setArtistId('null');
        }
    }, []);



    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        console.log("begin");

        if (!image) {
            setMessage('Please select an image file');
            return;
        }
        if (!music) {
            setMessage('Please select an audio file');
            return;
        }

        const formData = new FormData();
        formData.append('fileMusic', music);
        formData.append('fileImage', image);
        formData.append('musicTitle', title);
        formData.append('musicPlays', "0");
        formData.append('musicDuration', "3.14");
        formData.append('albumId', album);
        formData.append('artistId', artistId);
        formData.append('genreId', genre);
        console.log('Album ID: '  +  album)
        try {
            setLoading(true);
            const response = await axiosInstance.post('/music-service/api/Music/add', formData);
            console.log(response);

            if (response.status === 200) {
                setLoading(false);
                setMessage('Music uploaded successfully');
                setAlert(true)
            } else {
                setLoading(false);
                setMessage('Failed to upload music');
                setAlert(true)
            }
        } catch (error) {
            setLoading(false);
            console.error('Error uploading file:', error);
            setMessage('An error occurred while uploading the file');
        } finally {
            setLoading(false);
            console.log("end");
        }
    };



    useEffect(() => {
        const getGenres = async () => {
            const genreData = await axiosInstance.get(`/music-service/api/Genre/GetAllGenres`);
            setGenres(genreData.data.$values); 
        };

        getGenres();
    }, []);
    console.log(genres)
    useEffect(() => {
        const getAlbums = async () => {
            if (!artistId) {
                console.error("Artist ID is null or undefined");
                return;
            }

            try {
                const response = await axiosInstance.get(`/music-service/api/Album/getAllArtistAlbums/${artistId}`);
                console.log(response);
                setAlbums(response.data.$values);
            } catch (error) {
                console.error("Error fetching albums:", error);
            }
        };

        getAlbums();
    }, [artistId]);
    const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedGenre(event.target.value);
    };
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
                <div className="pb-12 pt-16 sm:pb-4 lg:pt-12">
                    <div className="lg:px-8">
                        <div className="lg:max-w-4xl">
                            <div className="mx-auto px-4 sm:px-6 md:max-w-2xl md:px-4 lg:px-0">
                                <h1 className="text-2xl font-bold leading-7 text-slate-900">Upload</h1>
                            </div>
                        </div>
                    </div>
                    <div className="divide-y divide-slate-100 sm:mt-4 lg:mt-8 lg:border-t lg:border-slate-100">
                        <div className="mx-auto max-w-270 mt-20">
                            <div className="grid grid-cols-5 gap-8">

                                <div className="col-span-5 xl:col-span-3">
                                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                                        <div className="border-b border-stroke px-7 py-4 dark:border-strokedark">
                                            <h3 className="font-medium text-black dark:text-white">
                                                Information
                                            </h3>
                                        </div>
                                        <div className="p-7">
                                            <div className="mb-5.5 flex flex-col gap-5.5 sm:flex-row">
                                                <div className="w-full sm:w-1/2">
                                                    <label
                                                        className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                        htmlFor="title"
                                                    >
                                                        Title
                                                    </label>
                                                    <div className="relative">

                                                        <input
                                                            className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                                                            type="text"
                                                            name="title"
                                                            id="title"
                                                            placeholder="Title of your song"
                                                            onChange={(e) => {
                                                                setTitle(e.target.value);
                                                            }}
                                                        />
                                                    </div>
                                                </div>

                                                <div className="w-full sm:w-1/2">
                                                    <label
                                                        className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                        htmlFor="tag"
                                                    >
                                                        Addtional Tags
                                                    </label>
                                                    <input
                                                        className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                                                        type="text"
                                                        name="tag"
                                                        id="tag"
                                                        placeholder="Tags"
                                                    />
                                                </div>
                                            </div>
                                            <div className="mb-5.5">
                                                <label
                                                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                    htmlFor="emailAddress"
                                                >
                                                    Select your album
                                                </label>
                                                <div className="relative">
                                                    <select onChange={(e) => {
                                                        setAlbum(e.target.value);
                                                    }} id="album" name="album" autoComplete="album-name" className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary">
                                                         {albums.map((album) => (
                                                            <option key={album.id} value={album.id}>{album.albumTitle}</option>
                                                         ))}
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="mb-5.5">
                                                <label
                                                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                    htmlFor="emailAddress"
                                                >
                                                    Genre
                                                </label>
                                                <div className="relative">
                                                    <select onChange={(e) => {
                                                        setGenre(e.target.value);
                                                    }} id="genre" name="genre" autoComplete="genre-name" className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary">
                                                         {genres.map((genre) => (
                                                            <option key={genre.id} value={genre.id}>{genre.genreName}</option>
                                                         ))}
                                                    </select>
                                                </div>
                                            </div>

                                            <div className="mb-5.5">
                                                <label
                                                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                    htmlFor="Caption"
                                                >
                                                    Caption
                                                </label>
                                                <input
                                                    className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                                                    type="text"
                                                    name="Caption"
                                                    id="Caption"
                                                    placeholder="Add a caption to your song"
                                                />
                                            </div>

                                            <div className="mb-5.5">
                                                <label
                                                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                    htmlFor="Username"
                                                >
                                                    Description
                                                </label>
                                                <div className="relative">
                                                    <textarea
                                                        className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                                                        name="bio"
                                                        id="bio"
                                                        rows={6}
                                                        placeholder="Describe your song"

                                                    ></textarea>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div className="col-span-5 xl:col-span-2">
                                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                                        <div className="border-b border-stroke px-7 py-4 dark:border-strokedark">
                                            <h3 className="font-medium text-black dark:text-white">
                                                Your Song
                                            </h3>
                                        </div>
                                        <div className="p-7">

                                            <div className="mb-4 flex items-center gap-3">
                                                <div className="h-14 w-14 rounded-full">
                                                    {preview ? (
                                                        <Image
                                                            src={preview}
                                                            width={55}
                                                            height={55}
                                                            alt="User"
                                                            className="rounded-full"
                                                        />
                                                    ) : (
                                                        <Image
                                                            src={'/21.jpg'}
                                                            width={55}
                                                            height={55}
                                                            alt="User"
                                                            className="rounded-full"
                                                        />
                                                    )}
                                                </div>
                                                <div>
                                                    <span className="mb-1.5 text-black dark:text-white">
                                                        Edit image
                                                    </span>
                                                    <span className="flex gap-2.5">
                                                        <label className="text-sm hover:text-primary text-purple-500 cursor-pointer">
                                                            Upload image
                                                            <input
                                                                type="file"
                                                                accept="image/*"
                                                                onChange={handleImage}
                                                                className="hidden"
                                                            />
                                                        </label>
                                                    </span>
                                                </div>
                                            </div>

                                            <div
                                                id="FileUpload"
                                                className="relative mb-5.5 block w-full cursor-pointer appearance-none rounded border border-dashed border-primary bg-gray px-4 py-4 dark:bg-meta-4 sm:py-7.5"
                                            >
                                                <input
                                                    type="file"
                                                    accept="audio/*"
                                                    className="absolute inset-0 z-50 m-0 h-full w-full cursor-pointer p-0 opacity-0 outline-none"
                                                    onChange={handleMusic}
                                                />
                                                <div className="flex flex-col items-center justify-center space-y-3">
                                                    <span className="flex h-10 w-10 items-center justify-center rounded-full border border-stroke bg-white dark:border-strokedark dark:bg-boxdark">
                                                        <svg
                                                            width="16"
                                                            height="16"
                                                            viewBox="0 0 16 16"
                                                            fill="none"
                                                            xmlns="http://www.w3.org/2000/svg"
                                                        >
                                                            <path
                                                                fillRule="evenodd"
                                                                clipRule="evenodd"
                                                                d="M1.99967 9.33337C2.36786 9.33337 2.66634 9.63185 2.66634 10V12.6667C2.66634 12.8435 2.73658 13.0131 2.8616 13.1381C2.98663 13.2631 3.1562 13.3334 3.33301 13.3334H12.6663C12.8431 13.3334 13.0127 13.2631 13.1377 13.1381C13.2628 13.0131 13.333 12.8435 13.333 12.6667V10C13.333 9.63185 13.6315 9.33337 13.9997 9.33337C14.3679 9.33337 14.6663 9.63185 14.6663 10V12.6667C14.6663 13.1971 14.4556 13.7058 14.0806 14.0809C13.7055 14.456 13.1968 14.6667 12.6663 14.6667H3.33301C2.80257 14.6667 2.29387 14.456 1.91879 14.0809C1.54372 13.7058 1.33301 13.1971 1.33301 12.6667V10C1.33301 9.63185 1.63148 9.33337 1.99967 9.33337Z"
                                                                fill="#3C50E0"
                                                            />
                                                            <path
                                                                fillRule="evenodd"
                                                                clipRule="evenodd"
                                                                d="M7.5286 1.52864C7.78894 1.26829 8.21106 1.26829 8.4714 1.52864L11.8047 4.86197C12.0651 5.12232 12.0651 5.54443 11.8047 5.80478C11.5444 6.06513 11.1223 6.06513 10.8619 5.80478L8 2.94285L5.13807 5.80478C4.87772 6.06513 4.45561 6.06513 4.19526 5.80478C3.93491 5.54443 3.93491 5.12232 4.19526 4.86197L7.5286 1.52864Z"
                                                                fill="#3C50E0"
                                                            />
                                                            <path
                                                                fillRule="evenodd"
                                                                clipRule="evenodd"
                                                                d="M7.99967 1.33337C8.36786 1.33337 8.66634 1.63185 8.66634 2.00004V10C8.66634 10.3682 8.36786 10.6667 7.99967 10.6667C7.63148 10.6667 7.33301 10.3682 7.33301 10V2.00004C7.33301 1.63185 7.63148 1.33337 7.99967 1.33337Z"
                                                                fill="#3C50E0"
                                                            />
                                                        </svg>
                                                    </span>
                                                    <p className="text-black">
                                                        <span className="text-black">Click to upload</span> or
                                                        drag and drop
                                                    </p>
                                                    <p className="mt-1.5 text-black">WAV, MP3, AAC or HLS</p>
                                                    <p className="text-black">(max, 10mb)</p>
                                                </div>
                                            </div>

                                            <div className="flex justify-end gap-4.5">
                                                <button
                                                    className="flex justify-center rounded border border-stroke px-6 py-2 font-medium text-black hover:shadow-1 dark:border-strokedark dark:text-white"
                                                    type="submit"
                                                >
                                                    Cancel
                                                </button>
                                                <button
                                                    onClick={(e) => {
                                                        setLoading(true)
                                                        handleSubmit(e)
                                                    }}
                                                    className="flex justify-center rounded lg:bg-purple-600 px-6 py-2 font-medium text-gray hover:bg-opacity-90"
                                                    type="submit"
                                                >
                                                    Upload
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <Transition show={loading} as={Fragment}>
                <Dialog
                    as="div"
                    className="relative z-10"
                    initialFocus={cancelButtonRef}
                    onClose={() => { }}
                >
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
                                    <div className="bg-purple-100 px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                                        <div className="sm:flex sm:items-start">
                                            <div className="bg-white mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full sm:mx-0 sm:h-10 sm:w-10">
                                                <CodeBracketIcon className="h-6 w-6 text-purple-600" aria-hidden="true" />
                                            </div>
                                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                                <h3 className="text-black font-semibold leading-6 text-gray-900">
                                                    Loading ...
                                                </h3>
                                                <div className="mt-2">
                                                    <p className="text-sm text-black">
                                                        Your request is being processed. Please wait a moment.
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </Dialog.Panel>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition>
            <Transition.Root show={alert} as={Fragment}>
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
                                                    {message}
                                                </Dialog.Title>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                                        <Link href={"/artist"}>
                                        <button
                                            type="button"
                                            className="inline-flex w-full justify-center rounded-md lg:bg-purple-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-purple-500 sm:ml-3 sm:w-auto"
                                            onClick={() => {
                                                setAlert(false)
                                            }}
                                        >
                                            Close
                                        </button>
                                        </Link>
                                    </div>
                                </Dialog.Panel>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition.Root>
        </div>
    );
};

export default Upload;
