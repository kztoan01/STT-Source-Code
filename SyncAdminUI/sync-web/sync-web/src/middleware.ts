import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import { JwtPayload, jwtDecode } from 'jwt-decode';
import Cookies from 'js-cookie';

interface MyJwtPayload extends JwtPayload {
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
}

export function middleware(request: NextRequest) {
    const token = request.cookies.get("token")?.value;
    console.log('Token:', token);

    if (!token) {
        console.info('No token found, redirecting to homepage.');
        return NextResponse.redirect(new URL('/', request.url));
    }

    try {
        const decodedToken = jwtDecode<MyJwtPayload>(token);
        console.log('Decoded Token:', decodedToken);

        const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        console.log('Role:', role);

        if (request.nextUrl.pathname.startsWith('/dashboard')) {
            if (!role || role.toLowerCase() !== 'admin') {
                console.info('Not admin, redirecting to homepage.');
                return NextResponse.redirect(new URL('/', request.url));
            }
        } else if (request.nextUrl.pathname.startsWith('/artist')) {
            if (!role || role.toLowerCase() !== 'artist') {
                console.info('Not artist, redirecting to homepage.');
                return NextResponse.redirect(new URL('/', request.url));
            }
        }
    } catch (error) {
        console.error('Error decoding token:', error);
        return NextResponse.redirect(new URL('/', request.url));
    }

    console.info('Access granted, proceeding.');
    return NextResponse.next();
}

export const config = {
    matcher: ['/dashboard', '/artist']
};
