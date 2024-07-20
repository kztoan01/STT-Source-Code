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
import ChartOne from "@/components/Charts/ChartOne";
import ChartTwo from "@/components/Charts/ChartTwo";
import ChatCard from "@/components/Chat/ChatCard";
import TableMusic from "@/components/Tables/TableMusic";
import { Chat } from "@/types/chat";
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

interface Participant {
    userName: string;
}

interface Playlist {
    musicName: string;
}

interface RoomDetails {
    name: string;
    code: string;
    hostId: string;
    image: string;
    participants: {
        $values: Participant[];
    };
    roomPlaylists: {
        $values: Playlist[];
    };
}
interface MyJwtPayload extends JwtPayload {
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
}
interface Music {
    id: string;
    musicTitle: string;
    musicUrl: string;
    musicPicture: string;
    musicPlays: number;
    musicDuration: number;
    genreName: string;
    artistName: string;
}
function AlbumDetail({ params }: { params: { roomId: string } }) {
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
    const [chatData, setChatData] = useState<Chat[]>([

    ]);

    const [chatBoxData, setChatBoxData] = useState<Chat[]>([

    ]);

    const [noti, setChatNoti] = useState<Chat[]>([

    ]);
    useEffect(() => {
        if (audio) {
            audio.pause();
            audio.src = source;
            if (isPlaying) {
                audio.play();
            } else {
                audio.pause();
            }
        } else if (source) {
            const newAudio = new Audio(source);
            setAudio(newAudio);
        }

        return () => {
            if (audio) {
                audio.pause();
                audio.src = '';
                setAudio(null);
            }
        };
    }, [source, isPlaying]);

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
            setIsPlaying(false);
        } else {
            audio.play();
            setIsPlaying(true);
        }
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

    const handleAdd = async (musicID: String) => {

        try {
            const response = await axiosInstance.post(`/room-service/api/Room/music/add`, {
                roomId: params.roomId,
                musicId: musicID
            });
            console.log(response);

            if (response.status === 200) {

            } else {

            }
        } catch (error) {
            console.error('Error uploading file:', error);
        } finally {
            console.log("end");
        }
    };
    const handleDelete = async (musicID: String) => {

        try {
            const response = await axiosInstance.post(`/room-service/api/Room/music/remove`, {
                roomId: params.roomId,
                musicId: musicID
            });
            console.log(response);

            if (response.status === 200) {

            } else {

            }
        } catch (error) {
            console.error('Error uploading file:', error);
        } finally {
            console.log("end");
        }
    };
    const [allMusics, setMusicList] = useState<Music[]>([]);
    const fetchAllMusic = async () => {
        const token = getCookie('token');
        console.log('Token:', token);

        if (typeof token === 'string') {
            try {
                const decoded = jwtDecode<MyJwtPayload>(token);
                console.log('Decoded Token:', decoded);

                const id = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
                setArtistId(id);
                console.log('Id:', id);
                console.log('Artist Id:', artistId);
                const response = await axiosInstance.get(`/music-service/api/Music/all`);
                setMusicList(response.data.$values);
            } catch (error) {
                console.error('Failed to decode token or fetch music:', error);
            }
        } else {
            setArtistId('null');
        }
    };

    useEffect(() => {
        fetchAllMusic();
    }, []);
    ////ROOOMMMMM

    const [artistId, setArtistId] = useState<string>('');
    const [roomDetails, setRoomDetails] = useState<RoomDetails | null>(null);
    const [isHost, setIsHost] = useState(false);
    const [isJoin, setIsJoin] = useState(false);
    const [isSync, setIsSync] = useState(false);
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [roomName, setRoomName] = useState<string>('');
    const [username, setUsername] = useState<string>('');
    const [chatContent, setChatContent] = useState<string>('');
    const [participants, setParticipants] = useState<any[]>([]);
    const [musics, setMusics] = useState<any[]>([]);
    const [newUser, setNewUser] = useState<string>('');
    const [loading, setLoading] = useState(false)
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

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://13.211.134.159:8080/roomhub')
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        newConnection.on('alertToRoom', (groupName, username) => {
            console.log("new user:" + username);
            setNewUser(username);
            setChatData(prevChatData => [
                ...prevChatData,
                {
                    avatar: "/default.jpg",
                    name: username,
                    text: isHost ? "Host" : "Member",
                    time: Date.now(),
                    textCount: 0,
                    dot: 5,
                }
            ]);
            setChatNoti(prevChatData => {
                const newChatEntry: Chat = {
                    avatar: "/default.jpg",
                    name: username,
                    text: "Just joined the room",
                    time: 0,
                    textCount: 0,
                    dot: 5,
                };

                const updatedChatData = [...prevChatData, newChatEntry];

                if (updatedChatData.length > 10) {
                    updatedChatData.shift();
                }

                return updatedChatData;
            });
        });
        newConnection.on('chat', (groupName, username, content) => {
            console.log("chat from" + username + "content: " + content);
            //setNewUser(username);
            setChatBoxData(prevChatData => {
                const newChatEntry: Chat = {
                    avatar: "/default.jpg",
                    name: content,
                    text: username,
                    time: Date.now(),
                    textCount: 0,
                    dot: 5,
                };

                const updatedChatData = [...prevChatData, newChatEntry];

                if (updatedChatData.length > 8) {
                    updatedChatData.shift();
                }

                return updatedChatData;
            });
            console.log(chatData)

        });
        newConnection.on('onLeaveRoom', (groupName, username) => {
            console.info(username);
            const testElement = document.getElementById('test');
            if (testElement) {
                testElement.innerHTML = `User ${username} has left the room ${groupName}.`;
            }
        });

        newConnection.on('updateParticipantsList', async () => {
            try {
                const response = await axiosInstance.get(`/room-service/api/Room/${params.roomId}`);
                const room = response.data;
                setParticipants(room.participants?.$values || []);

            } catch (error) {
                console.error('Error fetching participants:', error);
            }
        });

        newConnection.on('updateMusicsList', async () => {
            try {
                const response = await axiosInstance.get(`/room-service/api/Room/${params.roomId}`);
                const room = response.data;
                setMusics(room.roomPlaylists?.$values || []);

            } catch (error) {
                console.error('Error fetching musics:', error);
            }
        });

        newConnection.on('onAddRoomMusic', (groupName, musicName) => {
            console.info(musicName);
            const testElement = document.getElementById('test');
            if (testElement) {
                testElement.innerHTML = `Music ${musicName} has been added to the room ${groupName}.`;
            }
            setChatNoti(prevChatData => {
                const newChatEntry: Chat = {
                    avatar: "/default.jpg",
                    name: musicName,
                    text: `Music: ${musicName} has been added to the room.`,
                    time: 0,
                    textCount: 0,
                    dot: 5,
                };

                const updatedChatData = [...prevChatData, newChatEntry];

                if (updatedChatData.length > 10) {
                    updatedChatData.shift();
                }

                return updatedChatData;
            });
        });

        newConnection.on('musicStatus', (status, musicName, musicUrl) => {
            
            setTitle(musicName);
            setSource(musicUrl);
            console.log("rc musicName" + musicName)
            console.log("rc musicUrl" + musicUrl)
            if (status === 'play') {
                setIsPlaying(true);
            } else if (status === 'stop') {
                setIsPlaying(false);
            }
        });

        newConnection.onclose(() => {
            newConnection.start().catch(err => console.log('Error reconnecting to SignalR:', err));
        });

        newConnection.start()
            .then(() => {
                console.log('SignalR Connected.');
            })
            .catch(err => console.log('Error connecting to SignalR:', err));
    }, [params.roomId]);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Connected to SignalR');
                })
                .catch((err) => console.log('Connection failed: ', err));
        }
    }, [connection]);

    //   useEffect(() => {
    //     fetchRoomDetails()
    //   }, [artistId]);
    const musicIdsSet = new Set(musics.map(music => music.musicId));

    const uniqueMusics = allMusics.filter(music => !musicIdsSet.has(music.id));
    const convertTimestampToTime = (timestamp: any) => {
        const date = new Date(timestamp);
        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');
        return `${hours}:${minutes}`;
    };
    const reversedUniqueMusics = uniqueMusics.slice().reverse();
    const fetchRoomDetails = async () => {
        try {
            const response = await axiosInstance.get<RoomDetails>(`/room-service/api/Room/${params.roomId}`);
            console.log(response.data);
            setMusics(response.data.roomPlaylists?.$values || []);
            setParticipants(response.data.participants?.$values || []);
            setRoomDetails(response.data);
            if (response.data.hostId === artistId) {
                setIsHost(true);
            }
            setIsJoin(true)
        } catch (error) {
            console.error('Error fetching room details:', error);
        }
    };

    const handleJoinRoom = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (connection) {
            await connection.invoke('JoinRoom', params.roomId);
            await connection.invoke('AlertToRoom', params.roomId, username);
            fetchRoomDetails();
            //setIsHost(true);
        }
    };

    const handlePlay = async () => {
        if (isHost && connection && roomDetails) {
            setIsSync(true)
            console.log("title: " + title)
            console.log("source: " + source)
            await connection.invoke('MusicStatus', params.roomId, 'play', title, source);
        }
    };

    const handleStop = async () => {
        if (isHost && connection && roomDetails) {
            setIsSync(false)
            console.log("title: " + title)
            console.log("source: " + source)
            await connection.invoke('MusicStatus', params.roomId, 'stop', title, source);
        }
    };

    const handleChat = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (connection) {
            //await connection.invoke('JoinRoom', params.roomId);
            await connection.invoke('Chat', params.roomId, username, chatContent);
            //fetchRoomDetails();
            //setIsHost(true);
        }
    };
    return (
        <>
            <div>
                {!isJoin ? (
                    <form onSubmit={handleJoinRoom}>
                        <label htmlFor="username">Username:</label>
                        <input
                            type="text"
                            id="username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                        <button type="submit">Join Room</button>
                    </form>
                ) : <> {!roomDetails ? (
                    <div>
                        <input
                            type="text"
                            id="roomName"
                            value={roomName}
                            onChange={(e) => setRoomName(e.target.value)}
                            placeholder="Room Name"
                            required
                        />
                        {/* <button onClick={handleFetchRoomDetails}>Check Room</button> */}
                    </div>
                ) : (
                    <>
                        <article className="pt-12">
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
                                                    <h1 className="mt-2 text-4xl font-bold text-slate-900">{roomDetails.name}</h1><time
                                                        dateTime="2022-02-24T00:00:00.000Z"
                                                        className="order-first font-mono text-sm leading-7 text-slate-500">Room Detail</time>
                                                </div>

                                            </div>
                                            <p className="ml-24 mt-3 text-lg font-medium leading-8 text-slate-700">{roomDetails.code}</p>
                                            {isHost ? (
                                                <>
                                                    <div className="ml-24 mt-6 flex items-center gap-4">
                                                        <button type="button"
                                                            onClick={() => {
                                                                setLoading(true)
                                                            }}
                                                            aria-label="Play episode 5: Bill Lumbergh"
                                                            className="flex items-center gap-x-3 text-sm font-bold leading-6 text-pink-500 hover:text-pink-700 active:text-pink-900"><svg
                                                                aria-hidden="true" viewBox="0 0 10 10"
                                                                className="h-2.5 w-2.5 fill-current">
                                                                <path
                                                                    d="M8.25 4.567a.5.5 0 0 1 0 .866l-7.5 4.33A.5.5 0 0 1 0 9.33V.67A.5.5 0 0 1 .75.237l7.5 4.33Z">
                                                                </path>
                                                            </svg><span aria-hidden="true">Add song</span></button><span
                                                                aria-hidden="true"
                                                                className="text-sm font-bold text-slate-400">/</span>
                                                        <button type="button"
                                                            onClick={() => {
                                                                // setMusicId(music.id)
                                                                setOpenAlbum(true)
                                                            }}
                                                            className="flex items-center text-sm font-bold leading-6 text-pink-500 hover:text-pink-700 active:text-pink-900"
                                                            aria-label="Show notes for episode 5: Bill Lumbergh">Delete Room</button></div>
                                                </>
                                            ) : (<></>)}


                                        </header>
                                        <hr className="my-12 border-gray-200" />

                                    </div>
                                </div>
                            </div>
                        </article>
                        {/* <h1>{isHost ? "is host" : "not host"} check</h1>
                        <h1>{newUser} has join the room</h1>
                        <h3>Room Details</h3>
                        <div>
                            <strong>Room Name:</strong> {roomDetails.name}
                        </div>
                        <div>
                            <strong>Room Code:</strong> {roomDetails.code}
                        </div>
                        <div>
                            <strong>Host ID:</strong> {roomDetails.hostId}
                        </div>
                        <div>
                            <strong>Image:</strong>
                            <img src={roomDetails.image} alt="Room" style={{ width: '100px', height: '100px' }} />
                        </div>
                        <h4>Participants</h4>
                        <ul>
                            {participants?.map((participant) => (
                                <li key={participant.userName}>{participant.userName}</li>
                            ))}
                        </ul>
                        <h4>Music Playlist</h4>
                        <ul>
                            {musics?.map((playlist) => (
                                <li key={playlist.musicName}>{playlist.musicName}</li>
                            ))}
                        </ul> */}
                        {/* <div>
                            <audio id="audioPlayer" controls>
                                <source src="https://sync-music-storage.s3.amazonaws.com/music/d528c1bf-71da-4eea-9e2f-b05745785fbd.mp3" type="audio/mp3" />
                                Your browser does not support the audio element.
                            </audio>
                            <button onClick={handlePlay} disabled={!isHost}>Play</button>
                            <button onClick={handleStop} disabled={!isHost}>Stop</button>
                        </div> */}
                        <div className="mt-4 grid grid-cols-12 gap-4 md:mt-6 md:gap-6 2xl:mt-7.5 2xl:gap-7.5 pb-50">
                            {/* <ChartThree />
            <MapOne /> */}
                            <div className="col-span-12 xl:col-span-8">
                                {/* <TableMusic /> */}
                                <div className="rounded-sm border border-stroke bg-white px-5 pb-2.5 pt-6 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
                                    <h4 className="mb-6 text-xl font-semibold text-black dark:text-white">
                                        All Songs
                                    </h4>

                                    <div className="flex flex-col">
                                        <div className="rounded-sm bg-gray-2 dark:bg-meta-4 grid grid-cols-[60px_1fr_1fr] sm:grid-cols-[60px_1fr_1fr_1fr_1fr] gap-4">
                                            <div className="p-2.5 xl:p-5 w-8">
                                                <h5 className="text-sm font-medium uppercase xsm:text-base">
                                                    #
                                                </h5>
                                            </div>
                                            <div className="p-2.5 xl:p-5">
                                                <h5 className="text-sm font-medium uppercase xsm:text-base">
                                                    Song
                                                </h5>
                                            </div>

                                            <div className="p-2.5 text-center xl:p-5">
                                                <h5 className="text-sm font-medium uppercase xsm:text-base">
                                                    Add by
                                                </h5>
                                            </div>
                                            <div className="hidden p-2.5 text-center sm:block xl:p-5">
                                                <h5 className="text-sm font-medium uppercase xsm:text-base">
                                                    Album
                                                </h5>
                                            </div>
                                            <div className="hidden p-2.5 text-center sm:block xl:p-5">
                                                <h5 className="text-sm font-medium uppercase xsm:text-base">
                                                    Action
                                                </h5>
                                            </div>
                                        </div>

                                        {musics.slice().reverse().map((music, key) => (
                                            <div key={music.id}
                                                className={`grid grid-cols-[60px_1fr_1fr] sm:grid-cols-[60px_1fr_1fr_1fr_1fr] gap-4 `}

                                            >
                                                <div className="flex items-center p-2.5 xl:p-5 w-8">
                                                    {/* <p className="text-black dark:text-white">{musicList.indexOf(music)}</p> */}
                                                    <button onClick={() => {
                                                        if (isHost) {
                                                            setTitle(music.musicName)
                                                            setSource(music.musicUrl)
                                                        }

                                                        //togglePlay()
                                                    }}><svg viewBox="0 0 36 36" aria-hidden="true"
                                                        className="h-5 w-5 fill-black group-active:fill-white/80 md:h-7 md:w-7">
                                                            <path
                                                                d="M33.75 16.701C34.75 17.2783 34.75 18.7217 33.75 19.299L11.25 32.2894C10.25 32.8668 9 32.1451 9 30.9904L9 5.00962C9 3.85491 10.25 3.13323 11.25 3.71058L33.75 16.701Z">
                                                            </path>
                                                        </svg></button>
                                                </div>
                                                <div className="flex items-center gap-3 p-2.5 xl:p-5">
                                                    <div className="flex-shrink-0">
                                                        <img src={music.musicPicture} alt="21" width={48} height={48} className="rounded-full" />
                                                    </div>
                                                    <p className="hidden text-black dark:text-white sm:block">
                                                        {music.musicName}
                                                    </p>
                                                </div>


                                                <div className="flex items-center justify-center p-2.5 xl:p-5">
                                                    <p className="text-black dark:text-white">Added by host</p>
                                                </div>

                                                <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">
                                                    <p className="text-black dark:text-white">{music.albumName}</p>
                                                </div>
                                                <div className="col-span-1 flex items-center pl-20" onClick={() => {
                                                    handleDelete(music.musicId)
                                                }}>
                                                    <p
                                                        className="inline-flex rounded-full bg-opacity-10 px-3 py-1 text-sm font-medium bg-red text-red">
                                                        Remove
                                                    </p>
                                                </div>
                                            </div>
                                        ))}
                                    </div>

                                    <div className="fixed inset-x-0 bottom-0 z-10 lg:left-112 xl:left-120">
                                        <div
                                            className="flex items-center gap-6 bg-white/90 px-4 py-4 shadow shadow-slate-200/80 ring-1 ring-slate-900/5 backdrop-blur-sm md:px-6">
                                            <div className="hidden md:block"><button type="button"
                                                onClick={() => {
                                                    if (isHost) {
                                                        handlePlay()
                                                    }
                                                }}
                                                className="group relative flex h-10 w-10 flex-shrink-0 items-center justify-center rounded-full bg-slate-700 hover:bg-slate-900 focus:outline-none focus:ring-2 focus:ring-slate-700 focus:ring-offset-2 md:h-14 md:w-14"
                                                aria-label="Play">
                                                <div className="absolute -inset-3 md:hidden"></div><svg className="bg-white" fill="#000000" width="800px" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg"
                                                    viewBox="0 0 512 512" >
                                                    <path d="M256,0C114.617,0,0,114.615,0,256s114.617,256,256,256s256-114.615,256-256S397.383,0,256,0z M224,320
	                                                    c0,8.836-7.164,16-16,16h-32c-8.836,0-16-7.164-16-16V192c0-8.836,7.164-16,16-16h32c8.836,0,16,7.164,16,16V320z M352,320
	                                                    c0,8.836-7.164,16-16,16h-32c-8.836,0-16-7.164-16-16V192c0-8.836,7.164-16,16-16h32c8.836,0,16,7.164,16,16V320z"/>
                                                </svg>
                                            </button></div>
                                            <div className="hidden md:block"><button type="button"
                                                onClick={() => {
                                                    if (isHost) {
                                                        handleStop()
                                                    }
                                                }}
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
                                </div>
                            </div>

                            <div className="h-203 col-span-12 rounded-sm border border-stroke bg-white py-6 shadow-default dark:border-strokedark dark:bg-boxdark xl:col-span-4">
                                <h4 className="mb-6 px-7.5 text-xl font-semibold text-black dark:text-white">
                                    Chat
                                </h4>
                                <form onSubmit={handleChat} className="container px-8 flex">
                                    <div className="mb-5.5 container">
                                        <label
                                            className="mb-3 block text-sm font-medium text-black dark:text-white"
                                            htmlFor="Caption"
                                        >
                                            Chat
                                        </label>
                                        <input
                                            className="w-full rounded border border-stroke bg-gray px-4.5 py-3 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                                            type="text"
                                            name="Caption"
                                            id="Caption"
                                            placeholder="Sent a message"
                                            value={chatContent}
                                            onChange={(e) => setChatContent(e.target.value)}
                                        />
                                    </div>
                                    <button

                                        className="h-10 mt-9 ml-4 flex justify-center rounded lg:bg-purple-600 px-6 py-2 font-medium text-gray hover:bg-opacity-90"
                                        type="submit"
                                    >
                                        Send
                                    </button>
                                </form>
                                <div>
                                    {chatBoxData.slice().reverse().map((chat, key) => (
                                        <div

                                            className="flex items-center gap-5 px-7.5 py-3 hover:bg-gray-3 dark:hover:bg-meta-4"
                                            key={key}
                                        >
                                            <div className="relative h-14 w-14 rounded-full">
                                                <Image
                                                    width={56}
                                                    height={56}
                                                    src={chat.avatar}
                                                    alt="User"
                                                    style={{
                                                        width: "auto",
                                                        height: "auto",
                                                    }}
                                                />
                                                <span
                                                    className={`absolute bottom-0 right-0 h-3.5 w-3.5 rounded-full border-2 border-white ${chat.dot === 6 ? "bg-meta-6" : `bg-meta-3`
                                                        } `}
                                                ></span>
                                            </div>

                                            <div className="flex flex-1 items-center justify-between">
                                                <div>
                                                    <h5 className="font-medium text-black dark:text-white">
                                                        {chat.name}
                                                    </h5>
                                                    <p>
                                                        <span className="text-sm text-black dark:text-white">
                                                            From: {chat.text}
                                                        </span>
                                                        <span className="text-xs"> at {convertTimestampToTime(chat.time)}</span>
                                                    </p>
                                                </div>
                                                {chat.textCount !== 0 && (
                                                    <div className="flex h-6 w-6 items-center justify-center rounded-full bg-primary">
                                                        <span className="text-sm font-medium text-white">
                                                            {" "}
                                                            {chat.textCount}
                                                        </span>
                                                    </div>
                                                )}
                                            </div>
                                        </div>
                                    ))}


                                </div>

                            </div>

                            <div className="col-span-12 rounded-sm border border-stroke bg-white py-6 shadow-default dark:border-strokedark dark:bg-boxdark xl:col-span-4">
                                <h4 className="mb-6 px-7.5 text-xl font-semibold text-black dark:text-white">
                                    Participants
                                </h4>

                                <div>
                                    {chatData.slice().reverse().map((chat, key) => (
                                        <div

                                            className="flex items-center gap-5 px-7.5 py-3 hover:bg-gray-3 dark:hover:bg-meta-4"
                                            key={key}
                                        >
                                            <div className="relative h-14 w-14 rounded-full">
                                                <Image
                                                    width={56}
                                                    height={56}
                                                    src={chat.avatar}
                                                    alt="User"
                                                    style={{
                                                        width: "auto",
                                                        height: "auto",
                                                    }}
                                                />
                                                <span
                                                    className={`absolute bottom-0 right-0 h-3.5 w-3.5 rounded-full border-2 border-white ${chat.dot === 6 ? "bg-meta-6" : `bg-meta-3`
                                                        } `}
                                                ></span>
                                            </div>

                                            <div className="flex flex-1 items-center justify-between">
                                                <div>
                                                    <h5 className="font-medium text-black dark:text-white">
                                                        {chat.name}
                                                    </h5>
                                                    <p>
                                                        <span className="text-sm text-black dark:text-white">
                                                            {chat.text}
                                                        </span>
                                                        <span className="text-xs"> . at {convertTimestampToTime(chat.time)}</span>
                                                    </p>
                                                </div>
                                                {chat.textCount !== 0 && (
                                                    <div className="flex h-6 w-6 items-center justify-center rounded-full bg-primary">
                                                        <span className="text-sm font-medium text-white">
                                                            {" "}
                                                            {chat.textCount}
                                                        </span>
                                                    </div>
                                                )}
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>

                            <div className="col-span-12 rounded-sm border border-stroke bg-white py-6 shadow-default dark:border-strokedark dark:bg-boxdark xl:col-span-4">
                                <h4 className="mb-6 px-7.5 text-xl font-semibold text-black dark:text-white">
                                    Notifications
                                </h4>

                                <div>
                                    {noti.slice().reverse().map((chat, key) => (
                                        <div

                                            className="flex items-center gap-5 px-7.5 py-3 hover:bg-gray-3 dark:hover:bg-meta-4"
                                            key={key}
                                        >
                                            <div className="relative h-14 w-14 rounded-full">
                                                <Image
                                                    width={56}
                                                    height={56}
                                                    src={chat.avatar}
                                                    alt="User"
                                                    style={{
                                                        width: "auto",
                                                        height: "auto",
                                                    }}
                                                />
                                                <span
                                                    className={`absolute bottom-0 right-0 h-3.5 w-3.5 rounded-full border-2 border-white ${chat.dot === 6 ? "bg-meta-6" : `bg-meta-3`
                                                        } `}
                                                ></span>
                                            </div>

                                            <div className="flex flex-1 items-center justify-between">
                                                <div>
                                                    <h5 className="font-medium text-black dark:text-white">
                                                        {chat.name}
                                                    </h5>
                                                    <p>
                                                        <span className="text-sm text-black dark:text-white">
                                                            {chat.text}
                                                        </span>
                                                        <span className="text-xs"></span>
                                                    </p>
                                                </div>
                                                {chat.textCount !== 0 && (
                                                    <div className="flex h-6 w-6 items-center justify-center rounded-full bg-primary">
                                                        <span className="text-sm font-medium text-white">
                                                            {" "}
                                                            {chat.textCount}
                                                        </span>
                                                    </div>
                                                )}
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </>
                )}</>}

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
                                                                    Add music
                                                                </h3>
                                                            </div>
                                                            {reversedUniqueMusics.slice().reverse().map((music, key) => (
                                                                <div key={music.id}
                                                                    className={`grid grid-cols-[1fr] sm:grid-cols-[1fr] gap-4 w-80`}
                                                                    onClick={() => {
                                                                        setMusicId(music.id)
                                                                        handleAdd(music.id)
                                                                    }}
                                                                >
                                                                    <div className="flex items-center gap-3 p-2.5 xl:p-5">
                                                                        <div className="flex-shrink-0">
                                                                            <img src={music.musicPicture} alt="21" width={48} height={48} className="rounded-full" />
                                                                        </div>
                                                                        <p className="hidden text-black dark:text-white sm:block">
                                                                            {music.musicTitle}
                                                                        </p>
                                                                    </div>

                                                                </div>
                                                            ))}

                                                        </div>
                                                        <button
                                                            onClick={(e) => {
                                                                setLoading(false)
                                                            }}
                                                            className="ml-55 mt-4 flex justify-center rounded lg:bg-purple-600 px-6 py-2 font-medium text-gray hover:bg-opacity-90"
                                                            type="submit"
                                                        >
                                                            Close
                                                        </button>
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
            </div>

        </>
    );
};


export default AlbumDetail;
