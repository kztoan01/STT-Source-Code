import { useState, useEffect } from 'react';
import axiosInstance from "@/helpers/axiosInstance";

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

const useAlbums = (artistId: string) => {
  const [albums, setAlbums] = useState<Album[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAlbums = async () => {
      try {
        const response = await axiosInstance.get<ApiResponse>(`/music-service/api/Album/getAllArtistAlbums/${artistId}`);
        setAlbums(response.data.$values);
      } catch (error: any) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchAlbums();
  }, [artistId]);

  return { albums, loading, error };
};

export default useAlbums;
