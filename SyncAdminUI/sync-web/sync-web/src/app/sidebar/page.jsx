import Sidebar from "@/components/SideBar";
import SidebarItem from "@/components/SideBarItem";
import { BarChart3, Boxes, LayoutDashboard, Package, PackageX, Receipt } from "lucide-react";

export default function SideBarPage() {
  return (
    <Sidebar>
      <SidebarItem icon={<LayoutDashboard size={20} />} text="Dashboard" alert/>
      <SidebarItem icon={<BarChart3 size={20} />} text="Statistic" />
      <SidebarItem icon={<Boxes size={20} />} text="Inventory" />
      <SidebarItem icon={<Package size={20} />} text="Order" />
      <SidebarItem icon={<Receipt size={20} />} text="Billings" />
    </Sidebar>
    
  );
}