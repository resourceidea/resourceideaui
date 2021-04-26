import React from "react";
import { getLayout } from "@/layout/Shell";
import Dashboard from "@/layout/Dashboard";

const Home = () => {
  return <Dashboard />;
}

Home.getLayout = getLayout

export default Home
