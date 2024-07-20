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
import { useRouter } from "next/navigation";
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

interface RoomData {
    name: string;
    code: string;
    image: string;
  }
  
  interface User {
    userFullName: string;
    birthday: string;
    address: string;
    city: string;
    status: boolean;
    playlists: any[];
    followers: any[];
    musicHistories: any[];
    artist: any;
    room: any;
    participants: any[];
    id: string;
    userName: string;
    normalizedUserName: string;
    email: string;
    normalizedEmail: string;
    emailConfirmed: boolean;
    passwordHash: string;
    securityStamp: string;
    concurrencyStamp: string;
    phoneNumber: string | null;
    phoneNumberConfirmed: boolean;
    twoFactorEnabled: boolean;
    lockoutEnd: string | null;
    lockoutEnabled: boolean;
    accessFailedCount: number;
  }
  
  interface AddRoomResponse {
    id: string;
    name: string;
    user: User;
    code: string;
    image: string;
    hostId: string;
    roomPlaylists: any | null;
    participants: any | null;
  }
const Account = () => {

    const router = useRouter();
    const [image, setImage] = useState<File | null>(null);

    const [music, setMusic] = useState<File | null>(null);

    const [title, setTitle] = useState<string>('');
    const [description, setDescription] = useState<string>('');
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

        const formData = new FormData();
        formData.append('description', description);
        formData.append('image', image);
        try {
            setLoading(true);
            const response = await axiosInstance.put(`/music-service/api/Artist/updateArtistInfor/${artistId}`, formData);
            console.log(response);

            if (response.status === 200) {
                setLoading(false);
                setMessage('Account update successfully');
                setAlert(true)
            } else {
                setLoading(false);
                setMessage('Failed to update account');
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

    const [roomData, setRoomData] = useState({
        name: title,
        code: '',
        image: ''
      });
    
      const handleChangeRoom = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setRoomData({ ...roomData, [name]: value });
      };
    
      const handleSubmitRoom = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await axiosInstance.post<AddRoomResponse>('/room-service/api/Room/AddRoom',{
                name: title,
                code: '',
                image: ''
              });
              router.push(`/room/${response.data.id}`);
              console.log(response.data)
            return response.data;
          } catch (error) {
            console.error('Error adding room:', error);
            throw error;
          }
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
                                <h1 className="text-2xl font-bold leading-7 text-slate-900">Sync Room</h1>
                            </div>
                        </div>
                    </div>
                    <div className="divide-y divide-slate-100 sm:mt-4 lg:mt-8 lg:border-t lg:border-slate-100">
                        <div className="bg-gray-100">
                            <div className="mx-auto max-w-6xl px-4 sm:px-6 lg:px-8">
                                <div className="mx-auto max-w-2xl py-8 sm:py-8 lg:max-w-none lg:py-8">
                                    <div className="space-y-12 lg:grid lg:grid-cols-4 lg:gap-x-6 lg:space-y-0">

                                        <div className="col-span-1 group relative pb-8">
                                            <div className=" relative h-60 w-60 overflow-hidden rounded-lg bg-white sm:aspect-h-1 sm:aspect-w-2 lg:aspect-h-1 lg:aspect-w-1 group-hover:opacity-75 sm:h-64">

                                            </div>

                                        </div>
                                        <div className="col-span-1 group relative pb-8" onClick={() => setLoading(true)}>
                                            <div className=" relative h-60 w-60 overflow-hidden rounded-lg bg-white sm:aspect-h-1 sm:aspect-w-2 lg:aspect-h-1 lg:aspect-w-1 group-hover:opacity-75 sm:h-64">
                                                <img
                                                    src="https://t4.ftcdn.net/jpg/01/26/10/59/360_F_126105961_6vHCTRX2cPOnQTBvx9OSAwRUapYTEmYA.jpg"
                                                    alt="create"
                                                    className="h-full w-full object-cover object-center"
                                                />
                                            </div>

                                        </div>

                                        <div className="col-span-1 group relative pb-8">
                                            <div className=" relative h-60 w-60 overflow-hidden rounded-lg bg-white sm:aspect-h-1 sm:aspect-w-2 lg:aspect-h-1 lg:aspect-w-1 group-hover:opacity-75 sm:h-64">
                                                <img
                                                    src="https://t4.ftcdn.net/jpg/03/06/64/03/360_F_306640312_JeV1NsQu4MstyjuCrDRXlmrnXTT0jR8t.jpg"
                                                    alt="join"
                                                    className="h-full w-full object-cover object-center"
                                                />
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
                                        <div className="sm:flex sm:items-start pl-10">
                                            <div className="bg-white mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full sm:mx-0 sm:h-10 sm:w-10">
                                                <CodeBracketIcon className="h-6 w-6 text-purple-600" aria-hidden="true" />
                                            </div>
                                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                                <div className="col-span-5 xl:col-span-2">
                                                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                                                        <div className="border-b border-stroke px-7 py-4 dark:border-strokedark">
                                                            <h3 className="font-medium text-black dark:text-white">
                                                                New room
                                                            </h3>
                                                        </div>
                                                        <div className="p-7">
                                                            <div className="w-full pb-5">
                                                                <label
                                                                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                                                                    htmlFor="title"
                                                                >
                                                                    Enter room name
                                                                </label>
                                                                <div className="relative">

                                                                    <input
                                                                        className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                                                                        type="text"
                                                                        name="title"
                                                                        id="title"
                                                                        placeholder="Chilling"
                                                                        //value={roomData.name}
                                                                        onChange={(e) => {
                                                                            setTitle(e.target.value);
                                                                        }}
                                                                    />
                                                                </div>
                                                            </div>
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

                                                            

                                                            <div className="flex justify-end gap-4.5">
                                                                <button
                                                                onClick={() => setLoading(false)}
                                                                    className="flex justify-center rounded border border-stroke px-6 py-2 font-medium text-black hover:shadow-1 dark:border-strokedark dark:text-white"
                                                                    type="button"
                                                                >
                                                                    Cancel
                                                                </button>
                                                                <button
                                                                    onClick={(e) => {
                                                                        handleSubmitRoom(e)
                                                                    }}
                                                                    className="flex justify-center rounded lg:bg-purple-600 px-6 py-2 font-medium text-gray hover:bg-opacity-90"
                                                                    type="submit"
                                                                >
                                                                    Create
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
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

export default Account;
