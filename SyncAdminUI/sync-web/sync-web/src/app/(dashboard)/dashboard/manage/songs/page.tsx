
import React from "react";
import { Metadata } from "next";
import DefaultLayout from "@/components/Layouts/DefaultLayout";
import TableMusic from "@/components/Tables/TableMusic";
import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
export const metadata: Metadata = {
  title: "Manage Songs | SyncAdmin",
  description:
    "Manage Songs from Sync",
};


const FormElementsPage = () => {

  return (
    <DefaultLayout>
      {/* <FormElements /> */}
      <Breadcrumb pageName="Manage songs" />
      <div className="flex flex-col gap-10">
       <TableMusic/>
      </div>
    </DefaultLayout>
  );
};

export default FormElementsPage;
