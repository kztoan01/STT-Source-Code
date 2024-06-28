"use client"
import Link from 'next/link'
import { ChevronDownIcon } from '@heroicons/react/20/solid'
import { Switch } from '@headlessui/react'
import axios from 'axios'
import { useState, useEffect, ChangeEvent, FormEvent } from 'react'
import { Fragment, useRef } from 'react'
import { Dialog, Transition } from '@headlessui/react'
import { CheckCircleIcon, ExclamationTriangleIcon } from '@heroicons/react/24/outline'
import NavBar from '@/components/DashboardNav'
import logo from "../../../../../public/logo.jpg"
import Image from 'next/image'

export default function Upload() {

    // const [failopen, setFailOpen] = useState(false)
    // const [message, setMessage] = useState()
    // const [detailmessage, setDetailMessage] = useState()
    // const [category, setCategory] = useState("1")
    // const [language, setLanguage] = useState("English")
    // const [level, setLevel] = useState("Beginner")
    // const [price, setPrice] = useState("")
    // const [coupon, setCoupon] = useState("")
    // const [lo2, setLo2] = useState("")
    // const [lo1, setLo1] = useState("")
    // const [lo3, setLo3] = useState("")
    // const [lo4, setLo4] = useState("")
    // const [sec1, setSec1] = useState("")
    // const [sec2, setSec2] = useState("")
    // const [sec3, setSec3] = useState("")
    // const [sec4, setSec4] = useState("")
    // const [sec5, setSec5] = useState("")
    // const [sec6, setSec6] = useState("")
    // async function handleSubmit(e) {
    //     e.preventDefault()
    //     if (price > 0) {
    //         if (thisInstructor.isPremium == 0) {
    //             setMessage("Please complete the premium instructor application in order to set a price for your course.")
    //             setDetailMessage("You can set your course price as soon as your linked payment method is approved.")
    //             setFailOpen(true)
    //         } else {
    //             try {
    //                 await axios.post("https://arthubplatform1.azurewebsites.net/course/addCourse", {
    //                     accountId: thisInstructor.id,
    //                     status: 0,
    //                     isApproved: 0,
    //                     iPassed: 0,
    //                     coupon: coupon,
    //                     price: price,
    //                     language: language,
    //                     level: level,
    //                     introduction: introduction,
    //                     description: convertedContent,
    //                     name: name,
    //                     sections: [
    //                         {
    //                             name: sec1,
    //                             accountId: thisInstructor.id
    //                         },
    //                         {
    //                             name: sec2,
    //                             accountId: thisInstructor.id
    //                         },
    //                         {
    //                             name: sec3,
    //                             accountId: thisInstructor.id
    //                         },
    //                         {
    //                             name: sec4,
    //                             accountId: thisInstructor.id
    //                         },
    //                         {
    //                             name: sec5,
    //                             accountId: thisInstructor.id
    //                         },
    //                         {
    //                             name: sec6,
    //                             accountId: thisInstructor.id
    //                         }

    //                     ],
    //                     learningObjective: {
    //                         one: lo1,
    //                         two: lo2,
    //                         three: lo3,
    //                         four: lo4
    //                     },
    //                     categories: [
    //                         {
    //                             categoryId: category
    //                         }
    //                     ]
    //                 });
    //                 setOpen(true)

    //             } catch (err) {
    //                 alert(err);
    //             }
    //         }
    //     } else {
    //         try {
    //             await axios.post("https://arthubplatform1.azurewebsites.net/course/addCourse", {
    //                 accountId: thisInstructor.id,
    //                 status: 0,
    //                 isApproved: 0,
    //                 iPassed: 0,
    //                 coupon: coupon,
    //                 price: price,
    //                 language: language,
    //                 level: level,
    //                 introduction: introduction,
    //                 description: convertedContent,
    //                 name: name,
    //                 sections: [
    //                     {
    //                         name: sec1,
    //                         accountId: thisInstructor.id
    //                     },
    //                     {
    //                         name: sec2,
    //                         accountId: thisInstructor.id
    //                     },
    //                     {
    //                         name: sec3,
    //                         accountId: thisInstructor.id
    //                     },
    //                     {
    //                         name: sec4,
    //                         accountId: thisInstructor.id
    //                     },
    //                     {
    //                         name: sec5,
    //                         accountId: thisInstructor.id
    //                     },
    //                     {
    //                         name: sec6,
    //                         accountId: thisInstructor.id
    //                     }

    //                 ],
    //                 learningObjective: {
    //                     one: lo1,
    //                     two: lo2,
    //                     three: lo3,
    //                     four: lo4
    //                 },
    //                 categories: [
    //                     {
    //                         categoryId: category
    //                     }
    //                 ]
    //             });
    //             setOpen(true)

    //         } catch (err) {
    //             alert(err);
    //         }
    //     }

    // }
    const [agreed, setAgreed] = useState(false)

    const [image, setImage] = useState<File | null>(null);

    const [music, setMusic] = useState<File | null>(null);

    const [title, setTitle] = useState<string>('');

    const [genre, setGenre] = useState<string>('');

    const [message, setMessage] = useState<string>('');
    const handleImage = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setImage(e.target.files[0]);
        }
    }
    const handleMusic = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setMusic(e.target.files[0]);
        }
    }

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
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
        formData.append('albumId', "50da04ac-5f52-41d6-b11f-c85514270d2b");
        formData.append('artistId', "23cd5a02-6d0b-4153-a36d-fc4c16f44fc6");
        formData.append('genreId', "4c4df975-2874-4b63-bb64-d8c12cf65fee");
        try {
            const response = await fetch('https://localhost:7023/music-service/api/Music/add', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                setMessage('File uploaded successfully');
            } else {
                setMessage('Failed to upload file');
            }
        } catch (error) {
            console.error('Error uploading file:', error);
            setMessage('An error occurred while uploading the file');
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
            <div className="mx-auto max-w-2xl text-center mt-10">
                <h2 className="text-3xl font-bold tracking-tight text-gray-900 sm:text-4xl">Upload new music</h2>
                <p className="mt-2 text-lg leading-8 text-gray-600">
                    Tell us a little bit about your song.
                </p>
            </div>
            <form action="#" method="POST" className="mx-auto max-w-xl mt-6" onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 gap-x-8 gap-y-6 sm:grid-cols-2">
                    <div className="sm:col-span-3">
                        <div className="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-1 xl:gap-x-8">
                            <div className="relative">
                                <div className="aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80">
                                    <Image
                                        src={logo}
                                        alt=""
                                        className="h-full w-full object-cover object-center lg:h-full lg:w-full"
                                    />
                                </div>
                            </div>
                            <div className="flex text-sm leading-6 text-gray-600"> <label htmlFor="file-upload2"
                                className="relative cursor-pointer rounded-md bg-white font-semibold text-purple-600 focus-within:outline-none focus-within:ring-2 focus-within:ring-purple-600 focus-within:ring-offset-2 hover:text-purple-500">
                                <span>Upload music</span>
                                <input id="file-upload2" name="file-upload2" type="file" accept="audio/*" className="sr-only" onChange={handleMusic} />
                            </label>
                                <p className="pl-1">or drag and drop</p>

                            </div>
                        </div>
                    </div>
                    <div className="sm:col-span-2">
                        <label htmlFor="first-name" className="block text-sm font-semibold leading-6 text-gray-900">
                            Title
                        </label>
                        <div className="mt-2.5">
                            <input
                                type="text"
                                name="first-name"
                                id="first-name"
                                autoComplete="given-name"
                                className="block w-full rounded-md border-0 px-3.5 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-purple-600 sm:text-sm sm:leading-6"
                                onChange={(e) => {
                                    setTitle(e.target.value);
                                }}
                            />
                        </div>
                    </div>
                    <div className="sm:col-span-2">
                        <label htmlFor="company" className="block text-sm font-semibold leading-6 text-gray-900">
                            Addtional Tags
                        </label>
                        <div className="mt-2.5">
                            <input
                                type="text"
                                name="company"
                                id="company"
                                autoComplete="organization"
                                className="block w-full rounded-md border-0 px-3.5 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-purple-600 sm:text-sm sm:leading-6" />

                        </div>
                    </div>
                    <div className="sm:col-span-2">
                        <label htmlFor="company" className="block text-sm font-semibold leading-6 text-gray-900">
                            Description
                        </label>
                        <div className="mt-2.5">
                            <textarea
                                name="Description"
                                id="Description"
                                autoComplete="organization"
                                className="block w-full rounded-md border-0 px-3.5 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-purple-600 sm:text-sm sm:leading-6" />

                        </div>
                    </div>
                    <div className="sm:col-span-3">
                        <label htmlFor="genre" className="block text-sm font-medium leading-6 text-gray-900">Genre</label>
                        <div className="mt-2">
                            <select onChange={(e) => {
                                    setGenre(e.target.value);
                                }} id="genre" name="genre" autoComplete="genre-name" className="block w-full rounded-md border-0 px-3.5 py-2 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-purple-600 sm:max-w-xs sm:text-sm sm:leading-6">
                                <option value={"Pop"}>Pop</option>
                                <option value={"Rock"}>Rock</option>
                                <option value={"Hip Hop/Rap"}>Hip Hop/Rap</option>
                                <option value={"R&B/Soul"}>R&B/Soul</option>
                                <option value={"Country"}>Country</option>
                                <option value={"Electronic/Dance"}>Electronic/Dance</option>
                                <option value={"Jazz"}>Jazz</option>
                                <option value={"Blues"}>Blues</option>
                                <option value={"Reggae"}>Reggae</option>
                                <option value={"Latin"}>Latin</option>
                                <option value={"K-Pop"}>K-Pop</option>
                            </select>
                        </div>
                    </div>
                    <div className="sm:col-span-3">
                        <div className="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-1 xl:gap-x-8">
                            <div className="relative">
                                <div className="aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80">
                                    <Image
                                        src={logo}
                                        alt=""
                                        className="h-full w-full object-cover object-center lg:h-full lg:w-full"
                                    />
                                </div>

                                <div className="mt-4 flex justify-between">
                                    <div>
                                        <h3 className="text-sm font-medium text-gray-700">
                                            <a href="">
                                                <span aria-hidden="true" className="absolute inset-0" />
                                                {title}
                                            </a>
                                        </h3>
                                        <p className="mt-1 text-sm text-gray-500">Artist: 21 Savage</p>
                                    </div>
                                    <p className="text-sm text-gray-900">{genre}</p>
                                </div>
                            </div>
                            <div className="flex text-sm leading-6 text-gray-600"> <label htmlFor="file-upload1"
                                className="relative cursor-pointer rounded-md bg-white font-semibold text-purple-600 focus-within:outline-none focus-within:ring-2 focus-within:ring-purple-600 focus-within:ring-offset-2 hover:text-purple-500">
                                <span>Upload image</span>
                                <input id="file-upload1" name="file-upload1" type="file" className="sr-only"  onChange={handleImage}/>
                            </label>
                                <p className="pl-1">or drag and drop</p>

                            </div>
                            {/* <button
                        
                        type="button"
                        className="mt-10 flex w-1/3 items-center justify-center rounded-md border border-transparent lg:bg-purple-600 px-8 py-3 text-base font-medium text-white hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2"
                    >
                        Save Course Main Image
                    </button> */}
                        </div>
                    </div>

                    <Switch.Group as="div" className="flex gap-x-4 sm:col-span-2">
                        <div className="flex h-6 items-center">
                            <Switch
                                checked={agreed}
                                onChange={setAgreed}
                                className='bg-purple-600 flex w-8 flex-none cursor-pointer rounded-full p-px ring-1 ring-inset ring-gray-900/5 transition-colors duration-200 ease-in-out focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-purple-600'
                            >
                                <span className="sr-only">Agree to policies</span>
                                <span
                                    aria-hidden="true"
                                    className='translate-x-3.5 h-4 w-4 transform rounded-full bg-white shadow-sm ring-1 ring-gray-900/5 transition duration-200 ease-in-out'

                                />
                            </Switch>
                        </div>
                        <Switch.Label className="text-sm leading-6 text-gray-600">
                            By selecting this, you agree to our{' '}
                            <a href="#" className="font-semibold text-purple-600">
                                privacy&nbsp;policy
                            </a>
                            .
                        </Switch.Label>
                    </Switch.Group>
                </div>
                <div className="mt-10">
                    <h1>{message}</h1>
                    <button

                        type="submit"
                        className="block w-full rounded-md lg:bg-purple-600 px-3.5 py-2.5 text-center text-sm font-semibold text-white shadow-sm hover:bg-purple-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-purple-600"
                    >
                        Upload Music
                    </button>
                </div>
            </form>
        </div>
    )
}