import { openDB as idbOpenDB } from 'idb'

const DB_NAME = 'LocalSongsDB'
const STORE_NAME = 'songs'

export async function openDB() {
  return idbOpenDB(DB_NAME, 1, {
    upgrade(db) {
      if (!db.objectStoreNames.contains(STORE_NAME)) {
        db.createObjectStore(STORE_NAME)
      }
    }
  })
}

const dbPromise = openDB()

export async function saveSongToIndexedDB(id: number, file: File) {
  const db = await dbPromise
  await db.put(STORE_NAME, file, id)
}

export async function getSongFromIndexedDB(id: number): Promise<File | undefined> {
  const db = await dbPromise
  return await db.get(STORE_NAME, id)
}

export async function deleteSongFromIndexedDB(id: number) {
  const db = await dbPromise
  await db.delete(STORE_NAME, id)
}
