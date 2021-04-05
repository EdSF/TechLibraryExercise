<template>
  <div v-if="book">
    <div v-if="!show">
      <b-card
        :title="book.title"
        :img-src="book.thumbnailUrl"
        img-alt="Image"
        img-top
        tag="article"
        style="max-width: 30rem"
        class="mb-2"
      >
        <b-card-text>
          {{ book.descr }}
        </b-card-text>

        <b-button class="mr-2" to="/" variant="primary">Back</b-button>
        <b-button @click="showEdit" variant="warning"
          >Edit {{ book.title }}</b-button
        >
      </b-card>
    </div>

    <div v-if="book && show">
      <edit-book v-bind:book="book" />
    </div>
  </div>
</template>

<script>
import axios from "axios";
import EditBook from "./EditBook.vue";

export default {
  components: { EditBook },
  name: "Book",
  props: ["id"],
  data: () => ({
    book: null,
    show: false,
  }),
  mounted() {
    axios
      .get(`https://localhost:5001/books/book/${this.id}`)
      .then((response) => {
        this.book = response.data;
      });
  },

  methods: {
    showEdit() {
      this.show = !this.show;
    },
  },
};
</script>