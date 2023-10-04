const API_URL = "http://localhost:5240/api/chessgame";

export async function startRandomGameApi(token) {
   if (!token) {
      throw new Error("Unauthorized");
   }

   const response = await fetch(`${API_URL}/random`, {
      method: "POST",
      headers: {
         Authorization: `Bearer ${token}`,
         "Content-Type": "application/json",
      },
   });

   if (!response.ok) {
      const data = await response.json();
      throw new Error(data.message || "Error fetching data");
   }
   return await response.json();
}
export async function makeMoveApi(move, token) {
   if (!token) {
      throw new Error("Unauthorized");
   }

   const response = await fetch(`${API_URL}/move`, {
      method: "POST",
      headers: {
         Authorization: `Bearer ${token}`,
         "Content-Type": "application/json",
      },
      body: JSON.stringify(move),
   });

   if (!response.ok) {
      const data = await response.json();
      throw new Error(data.message || "Error fetching data");
   }
   return await response.json();
}
export async function getCurrentGameApi(token) {
   if (!token) {
      throw new Error("Unauthorized");
   }

   const response = await fetch(`${API_URL}/current-game`, {
      method: "GET",
      headers: {
         Authorization: `Bearer ${token}`,
         "Content-Type": "application/json",
      },
   });

   if (!response.ok) {
      const data = await response.json();
      throw new Error(data.message || "Error fetching data");
   }
   return await response.json();
}
