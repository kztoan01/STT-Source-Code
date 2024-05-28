// 'use client';

// import {
//   TableHead,
//   TableRow,
//   TableHeader,
//   TableCell,
//   TableBody,
//   Table
// } from '@/components/ui/table';
// import { Button } from '@/components/ui/button';
// import { SelectUser } from '@/lib/db';
// import { deleteUser } from './actions';
// import { useRouter } from 'next/navigation';

// export function UsersTable({
//   users,
//   offset
// }: {
//   users: SelectUser[];
//   offset: number | null;
// }) {
//   const router = useRouter();

//   function onClick() {
//     router.replace(`/?offset=${offset}`);
//   }

//   return (
//     <>
//       <form className="border shadow-sm rounded-lg">
//         <Table>
//           <TableHeader>
//             <TableRow>
//               <TableHead className="max-w-[150px]">Name</TableHead>
//               <TableHead className="hidden md:table-cell">Email</TableHead>
//               <TableHead className="hidden md:table-cell">Username</TableHead>
//               <TableHead></TableHead>
//             </TableRow>
//           </TableHeader>
//           <TableBody>
//             {users.map((user) => (
//               <UserRow key={user.id} user={user} />
//             ))}
//           </TableBody>
//         </Table>
//       </form>
//       {offset !== null && (
//         <Button
//           className="mt-4 w-40"
//           variant="secondary"
//           onClick={() => onClick()}
//         >
//           Next Page
//         </Button>
//       )}
//     </>
//   );
// }

// function UserRow({ user }: { user: SelectUser }) {
//   const userId = user.id;
//   const deleteUserWithId = deleteUser.bind(null, userId);

//   return (
//     <TableRow>
//       <TableCell className="font-medium">{user.name}</TableCell>
//       <TableCell className="hidden md:table-cell">{user.email}</TableCell>
//       <TableCell>{user.username}</TableCell>
//       <TableCell>
//         <Button
//           className="w-full"
//           size="sm"
//           variant="outline"
//           formAction={deleteUserWithId}
//           disabled
//         >
//           Delete
//         </Button>
//       </TableCell>
//     </TableRow>
//   );
// }

'use client';

import {
  TableHead,
  TableRow,
  TableHeader,
  TableCell,
  TableBody,
  Table
} from '@/components/ui/table';
import { Button } from '@/components/ui/button';
//import  usersData  from '../lib/user';
import { deleteUser } from './actions';
import { useRouter } from 'next/navigation';

export function UsersTable() {
  const router = useRouter();

  // function onClick() {
  //   router.replace(`/?offset=${offset}`);
  // }
  const usersData = [
    {
      "id": 1,
      "name": "Tran Bao Toan",
      "email": "kztoan01@gmail.com",
      "username": "toan.conquers"
    },
    {
      "id": 2,
      "name": "Nguyen Hiep Phu",
      "email": "phu@gmail.com",
      "username": "phu.conquers"
    },
    {
      "id": 3,
      "name": "Ta Gia Nhat Minh",
      "email": "minh@gmail.com",
      "username": "minh.conquers"
    },
    {
      "id": 4,
      "name": "Lam Le Nhan",
      "email": "nhan@gmail.com",
      "username": "nhan.conquers"
    },
  ]
  
  return (
    <>
      <form className="border shadow-sm rounded-lg">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="max-w-[150px]">Name</TableHead>
              <TableHead className="hidden md:table-cell">Email</TableHead>
              <TableHead className="hidden md:table-cell">Username</TableHead>
              <TableHead></TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {usersData.map((user) => (
              <UserRow key={user.id} user={user} />
            ))}
          </TableBody>
        </Table>
      </form>
      
        <Button
          className="mt-4 w-40"
          variant="secondary"
          //onClick={() => onClick()}
        >
          Next Page
        </Button>
      
    </>
  );
}

function UserRow({ user }: { user: { id: number, name: string, email: string, username: string } }) {
  const userId = user.id;
  const deleteUserWithId = deleteUser.bind(null, userId);

  return (
    <TableRow>
      <TableCell className="font-medium">{user.name}</TableCell>
      <TableCell className="hidden md:table-cell">{user.email}</TableCell>
      <TableCell>{user.username}</TableCell>
      <TableCell>
        <Button
          className="w-full"
          size="sm"
          variant="outline"
          formAction={deleteUserWithId}
          disabled
        >
          Delete
        </Button>
      </TableCell>
    </TableRow>
  );
}

