"use client"
import Image from "next/image";
import { Product } from "@/types/product";
import { useEffect, useState } from "react";
import axiosInstance from "@/helpers/axiosInstance";
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
type Music = {
  id: string;
  musicTitle: string;
  musicUrl: string;
  musicPicture: string;
  musicPlays: number;
  musicDuration: number;
  releaseDate: string;
  genreName: string;
  artistName: string;
}
type Album = {
  id: string;
  albumTitle: string;
  albumDescription: string;
  releaseDate: string;
  imageUrl: string;
};
type Musics = {
  $id: string;
  $values: Music[];
};
type ApiResponse = {
  $id: string;
  $values: Album[];
};

const TableArtist = () => {
  const [albums, setAlbums] = useState<Album[]>([]);
  const [musicList, setMusicList] = useState<Music[]>([]);
  const [artists, setArtists] = useState<any[]>([]);
  const getArtists = async () => {

    try {
      const response = await axiosInstance.get(`/music-service/api/Admin/getallartist`);
      console.log(response);
      setArtists(response.data.$values);
    } catch (error) {
      console.error("Error fetching artists:", error);
    }
  };

  useEffect(() => {
    getArtists();
  }, []);


  const [title, setTitle] = useState<string>('');
  const [source, setSource] = useState<string>('');
  const [audio, setAudio] = useState<HTMLAudioElement | null>(null);
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);

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

  return (
    <>
      <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
        <div className="px-4 py-6 md:px-6 xl:px-7.5">
          <h4 className="text-xl font-semibold text-black dark:text-white">
            All Artist
          </h4>
        </div>

        <div className="grid grid-cols-6 border-t border-stroke px-4 py-4.5 dark:border-strokedark sm:grid-cols-9 md:px-6 2xl:px-7.5">
          <div className="col-span-2 flex items-center">
            <p className="font-medium">Name</p>
          </div>
          <div className="col-span-3 flex items-center">
            <p className="font-medium">Description</p>
          </div>
          <div className="col-span-1 hidden items-center sm:flex">
            <p className="font-medium">Albums</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Musics</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Monthly listens</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Status</p>
          </div>
        </div>

        {artists.map((artist, key) => (
          <div
            className="grid grid-cols-6 border-t border-stroke px-4 py-4.5 dark:border-strokedark sm:grid-cols-9 md:px-6 2xl:px-7.5 cursor-pointer"
            key={key}
            onClick={() => {
                setMusicList(artist.viralMusics.$values)
                setAlbums(artist.albums.$values)
            }
            }
          >
            <div className="col-span-2 flex items-center">
              <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
                <div className="h-12.5 w-15 rounded-md">
                  <img
                    src={artist.artistImage}
                    width={60}
                    height={50}
                    alt="Product"
                  />
                </div>
                <p className="text-sm text-black dark:text-white">
                  {artist.artistName}
                </p>
              </div>
            </div>
            <div className="col-span-3 hidden items-center sm:flex">
              <p className="text-sm text-black dark:text-white line-clamp-2">
                {artist.artistDescription}
              </p>
            </div>
            <div className="col-span-1 hidden items-center sm:flex pl-5">
              <p className="text-sm text-black dark:text-white">
              {artist.albums.$values.length}
              </p>
            </div>
            <div className="col-span-1 flex items-center pl-5">
              <p className="text-sm text-black dark:text-white">
              {artist.viralMusics.$values.length}
              </p>
            </div>
            <div className="col-span-1 flex items-center">
              <p className="text-sm text-black dark:text-white">1,505,255</p>
            </div>
            <div className="col-span-1 flex items-center">
              <p
                className="inline-flex rounded-full bg-opacity-10 px-3 py-1 text-sm font-medium bg-success text-success">
                Available
              </p>
            </div>
          </div>
        ))}
      </div>
      <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
        <div className="px-4 py-6 md:px-6 xl:px-7.5">
          <h4 className="text-xl font-semibold text-black dark:text-white">
            Artist Albums
          </h4>
        </div>

        <div className="grid grid-cols-6 border-t border-stroke px-4 py-4.5 dark:border-strokedark sm:grid-cols-9 md:px-6 2xl:px-7.5">
          <div className="col-span-2 flex items-center">
            <p className="font-medium">Album</p>
          </div>
          <div className="col-span-3 flex items-center">
            <p className="font-medium">Description</p>
          </div>
          <div className="col-span-1 hidden items-center sm:flex">
            <p className="font-medium">Release Date</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Monthly listens</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="font-medium">Status</p>
          </div>
        </div>

        {albums.map((album, key) => (
          <div
            className="grid grid-cols-6 border-t border-stroke px-4 py-4.5 dark:border-strokedark sm:grid-cols-9 md:px-6 2xl:px-7.5 cursor-pointer"
            key={key}
          >
            <div className="col-span-2 flex items-center">
              <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
                <div className="h-12.5 w-15 rounded-md">
                  <img
                    src={album.imageUrl}
                    width={60}
                    height={50}
                    alt="Product"
                  />
                </div>
                <p className="text-sm text-black dark:text-white">
                  {album.albumTitle}
                </p>
              </div>
            </div>
            <div className="col-span-3 hidden items-center sm:flex">
              <p className="text-sm text-black dark:text-white">
                {album.albumDescription}
              </p>
            </div>
            <div className="col-span-1 hidden items-center sm:flex">
              <p className="text-sm text-black dark:text-white">
                {new Date(album.releaseDate).toLocaleDateString('en-US', {
                  year: 'numeric',
                  month: 'long',
                  day: 'numeric'
                })}
              </p>
            </div>
            <div className="col-span-1 flex items-center">
              <p className="text-sm text-black dark:text-white">1,505,255</p>
            </div>
            <div className="col-span-1 flex items-center">
              <p
                className="inline-flex rounded-full bg-opacity-10 px-3 py-1 text-sm font-medium bg-success text-success">
                Available
              </p>
            </div>
          </div>
        ))}
      </div>
      <div className="rounded-sm border border-stroke bg-white px-5 pb-2.5 pt-6 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
        <div className="max-w-full overflow-x-auto">
          <table className="w-full table-auto">
            <thead>
              <tr className="bg-gray-2 text-left dark:bg-meta-4">
                <th className="min-w-[220px] px-4 py-4 font-medium text-black dark:text-white xl:pl-11">
                  Title
                </th>
                <th className="min-w-[150px] px-4 py-4 font-medium text-black dark:text-white">
                  Genre
                </th>
                <th className="min-w-[120px] px-4 py-4 font-medium text-black dark:text-white">
                  Durations
                </th>
                <th className="px-4 py-4 font-medium text-black dark:text-white">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody>
              {musicList.map((music, key) => (
                <tr key={key}>
                  <td className="border-b border-[#eee] px-4 py-5 pl-9 dark:border-strokedark xl:pl-11">
                    <h5 className="font-medium text-black dark:text-white">
                      {music.musicTitle}
                    </h5>
                    <p className="text-sm">{music.artistName}</p>
                  </td>
                  <td className="border-b border-[#eee] px-4 py-5 dark:border-strokedark">
                    <p className="text-black dark:text-white">
                      {music.genreName}
                    </p>
                  </td>
                  <td className="border-b border-[#eee] px-4 py-5 dark:border-strokedark">
                    <p
                      className="inline-flex rounded-full bg-opacity-10 px-3 py-1 text-sm font-medium bg-success text-success"
                    >
                      Available
                    </p>
                  </td>
                  <td className="border-b border-[#eee] px-4 py-5 dark:border-strokedark">
                    <div className="flex items-center space-x-3.5">
                      <button className="hover:text-primary" onClick={() => {
                        setTitle(music.musicTitle)
                        setSource(music.musicUrl)
                      }}>
                        <svg
                          className="fill-current"
                          width="18"
                          height="18"
                          viewBox="0 0 18 18"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M8.99981 14.8219C3.43106 14.8219 0.674805 9.50624 0.562305 9.28124C0.47793 9.11249 0.47793 8.88749 0.562305 8.71874C0.674805 8.49374 3.43106 3.20624 8.99981 3.20624C14.5686 3.20624 17.3248 8.49374 17.4373 8.71874C17.5217 8.88749 17.5217 9.11249 17.4373 9.28124C17.3248 9.50624 14.5686 14.8219 8.99981 14.8219ZM1.85605 8.99999C2.4748 10.0406 4.89356 13.5562 8.99981 13.5562C13.1061 13.5562 15.5248 10.0406 16.1436 8.99999C15.5248 7.95936 13.1061 4.44374 8.99981 4.44374C4.89356 4.44374 2.4748 7.95936 1.85605 8.99999Z"
                            fill=""
                          />
                          <path
                            d="M9 11.3906C7.67812 11.3906 6.60938 10.3219 6.60938 9C6.60938 7.67813 7.67812 6.60938 9 6.60938C10.3219 6.60938 11.3906 7.67813 11.3906 9C11.3906 10.3219 10.3219 11.3906 9 11.3906ZM9 7.875C8.38125 7.875 7.875 8.38125 7.875 9C7.875 9.61875 8.38125 10.125 9 10.125C9.61875 10.125 10.125 9.61875 10.125 9C10.125 8.38125 9.61875 7.875 9 7.875Z"
                            fill=""
                          />
                        </svg>
                      </button>
                      <button className="hover:text-primary">
                        <svg
                          className="fill-current"
                          width="18"
                          height="18"
                          viewBox="0 0 18 18"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M13.7535 2.47502H11.5879V1.9969C11.5879 1.15315 10.9129 0.478149 10.0691 0.478149H7.90352C7.05977 0.478149 6.38477 1.15315 6.38477 1.9969V2.47502H4.21914C3.40352 2.47502 2.72852 3.15002 2.72852 3.96565V4.8094C2.72852 5.42815 3.09414 5.9344 3.62852 6.1594L4.07852 15.4688C4.13477 16.6219 5.09102 17.5219 6.24414 17.5219H11.7004C12.8535 17.5219 13.8098 16.6219 13.866 15.4688L14.3441 6.13127C14.8785 5.90627 15.2441 5.3719 15.2441 4.78127V3.93752C15.2441 3.15002 14.5691 2.47502 13.7535 2.47502ZM7.67852 1.9969C7.67852 1.85627 7.79102 1.74377 7.93164 1.74377H10.0973C10.2379 1.74377 10.3504 1.85627 10.3504 1.9969V2.47502H7.70664V1.9969H7.67852ZM4.02227 3.96565C4.02227 3.85315 4.10664 3.74065 4.24727 3.74065H13.7535C13.866 3.74065 13.9785 3.82502 13.9785 3.96565V4.8094C13.9785 4.9219 13.8941 5.0344 13.7535 5.0344H4.24727C4.13477 5.0344 4.02227 4.95002 4.02227 4.8094V3.96565ZM11.7285 16.2563H6.27227C5.79414 16.2563 5.40039 15.8906 5.37227 15.3844L4.95039 6.2719H13.0785L12.6566 15.3844C12.6004 15.8625 12.2066 16.2563 11.7285 16.2563Z"
                            fill=""
                          />
                          <path
                            d="M9.00039 9.11255C8.66289 9.11255 8.35352 9.3938 8.35352 9.75942V13.3313C8.35352 13.6688 8.63477 13.9782 9.00039 13.9782C9.33789 13.9782 9.64727 13.6969 9.64727 13.3313V9.75942C9.64727 9.3938 9.33789 9.11255 9.00039 9.11255Z"
                            fill=""
                          />
                          <path
                            d="M11.2502 9.67504C10.8846 9.64692 10.6033 9.90004 10.5752 10.2657L10.4064 12.7407C10.3783 13.0782 10.6314 13.3875 10.9971 13.4157C11.0252 13.4157 11.0252 13.4157 11.0533 13.4157C11.3908 13.4157 11.6721 13.1625 11.6721 12.825L11.8408 10.35C11.8408 9.98442 11.5877 9.70317 11.2502 9.67504Z"
                            fill=""
                          />
                          <path
                            d="M6.72245 9.67504C6.38495 9.70317 6.1037 10.0125 6.13182 10.35L6.3287 12.825C6.35683 13.1625 6.63808 13.4157 6.94745 13.4157C6.97558 13.4157 6.97558 13.4157 7.0037 13.4157C7.3412 13.3875 7.62245 13.0782 7.59433 12.7407L7.39745 10.2657C7.39745 9.90004 7.08808 9.64692 6.72245 9.67504Z"
                            fill=""
                          />
                        </svg>
                      </button>
                      <button className="hover:text-primary">
                        <svg
                          className="fill-current"
                          width="18"
                          height="18"
                          viewBox="0 0 18 18"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M16.8754 11.6719C16.5379 11.6719 16.2285 11.9531 16.2285 12.3187V14.8219C16.2285 15.075 16.0316 15.2719 15.7785 15.2719H2.22227C1.96914 15.2719 1.77227 15.075 1.77227 14.8219V12.3187C1.77227 11.9812 1.49102 11.6719 1.12539 11.6719C0.759766 11.6719 0.478516 11.9531 0.478516 12.3187V14.8219C0.478516 15.7781 1.23789 16.5375 2.19414 16.5375H15.7785C16.7348 16.5375 17.4941 15.7781 17.4941 14.8219V12.3187C17.5223 11.9531 17.2129 11.6719 16.8754 11.6719Z"
                            fill=""
                          />
                          <path
                            d="M8.55074 12.3469C8.66324 12.4594 8.83199 12.5156 9.00074 12.5156C9.16949 12.5156 9.31012 12.4594 9.45074 12.3469L13.4726 8.43752C13.7257 8.1844 13.7257 7.79065 13.5007 7.53752C13.2476 7.2844 12.8539 7.2844 12.6007 7.5094L9.64762 10.4063V2.1094C9.64762 1.7719 9.36637 1.46252 9.00074 1.46252C8.66324 1.46252 8.35387 1.74377 8.35387 2.1094V10.4063L5.40074 7.53752C5.14762 7.2844 4.75387 7.31252 4.50074 7.53752C4.24762 7.79065 4.27574 8.1844 4.50074 8.43752L8.55074 12.3469Z"
                            fill=""
                          />
                        </svg>
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
            
          </table>
          <div className="inset-x-0 bottom-0 z-10 lg:left-112 xl:left-120 mt-8">
              <div
                className="text-black dark:text-white flex items-center gap-6 bg-slate-800 px-4 py-4 shadow ring-1 ring-slate-900/5 backdrop-blur-sm md:px-6">
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
                  className="truncate text-center text-black dark:text-white  text-sm font-bold leading-6 md:text-left" title="1: Skeletor"
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
    </>
  );
};

export default TableArtist;
