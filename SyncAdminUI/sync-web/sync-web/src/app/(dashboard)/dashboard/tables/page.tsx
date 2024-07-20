import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
import TableThree from "@/components/Tables/TableThree";


import { Metadata } from "next";
import DefaultLayout from "@/components/Layouts/DefaultLayout";
import TableMusic from "@/components/Tables/TableMusic";
import TableAlbum from "@/components/Tables/TableAlbum";

export const metadata: Metadata = {
  title: "Next.js Tables | SyncAdmin - Next.js Dashboard Template",
  description:
    "This is Next.js Tables page for SyncAdmin - Next.js Tailwind CSS Admin Dashboard Template",
};

const TablesPage = () => {
  return (
    <DefaultLayout>
      <Breadcrumb pageName="Tables" />

      <div className="flex flex-col gap-10">
        <TableMusic />
        <TableAlbum />
        <TableThree />
      </div>
    </DefaultLayout>
  );
};

export default TablesPage;
