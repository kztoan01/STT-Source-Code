import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";

import { Metadata } from "next";
import DefaultLayout from "@/components/Layouts/DefaultLayout";
import SelectGroupOne from "@/components/SelectGroup/SelectGroupOne";
import Link from "next/link";
import TableMusic from "@/components/Tables/TableMusic";
import TableAlbum from "@/components/Tables/TableAlbum";
import TableThree from "@/components/Tables/TableThree";

export const metadata: Metadata = {
  title: "Next.js Form Layout | SyncAdmin - Next.js Dashboard Template",
  description:
    "This is Next.js Form Layout page for SyncAdmin - Next.js Tailwind CSS Admin Dashboard Template",
};

const FormLayout = () => {
  return (
    <DefaultLayout>
      <Breadcrumb pageName="Manage albums" />

      <div className="flex flex-col gap-10">
        <TableAlbum />
      </div>
    </DefaultLayout>
  );
};

export default FormLayout;
