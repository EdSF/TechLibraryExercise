<template>
  <div>
    <h1>Editing {{ book.title }}</h1>
    <b-form @submit="onSubmit" @reset="onReset">
      <b-form-group id="input-group-1" label="Book Title" label-for="title">
        <b-form-input
          id="title"
          v-model="form.title"
          placeholder="Title of the book"
          required
        ></b-form-input>
      </b-form-group>

      <b-form-group id="input-group-2" label="ISBN" label-for="isbn">
        <b-form-input
          id="isbn"
          v-model="form.isbn"
          placeholder="Book ISBN"
        ></b-form-input>
      </b-form-group>

      <b-form-group
        id="input-group-3"
        label="Date Published"
        label-for="publishDate"
      >
        <b-form-input
          id="publishDate"
          v-model="form.publishedDate"
          placeholder="Book publish date"
          type="date"
        ></b-form-input>
      </b-form-group>

      <b-form-group
        id="input-group-4"
        label="Thumbnail URL"
        label-for="thumbnail"
      >
        <b-form-input
          id="thumbnail"
          v-model="form.thumbnailUrl"
          placeholder="Thumbnail image url"
        ></b-form-input>
      </b-form-group>

      <b-form-group
        id="input-group-5"
        label="Short Description"
        label-for="description"
      >
        <b-form-textarea
          id="description"
          v-model="form.descr"
          rows="3"
          placeholder="short description"
        ></b-form-textarea>
      </b-form-group>

      <b-button class="mr-2" type="submit" variant="success">Submit</b-button>
      <b-button type="reset" variant="danger">Reset</b-button>
    </b-form>

    <div class="mt-5">
      <b-button to="/" variant="primary">Back</b-button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "EditForm",
  props: ["book"],
  data() {
    return {
      form: {
        title: this.book.title,
        isbn: this.book.isbn,
        publishedDate: this.book.publishedDate,
        thumbnailUrl: this.book.thumbnailUrl,
        descr: this.book.descr,
        bookId: this.book.bookId,
      },
      show: true,
    };
  },

  methods: {
    onSubmit(event) {
      event.preventDefault();
      var payload = JSON.stringify(this.form);
      console.info(payload);
      var vm = this;
      axios
        .put(`https://localhost:5001/books/${vm.book.bookId}`, payload, {
          headers: {
            "Content-Type": "application/json",
            Accept: "application/json",
          },
        })
        .then((res) => {
          console.info(res);
          this.$router.push({ path: "/" });
        })
        .catch(e =>{
            console.error(`Update failed: ${e}`)
        });
    },

    onReset(event) {
      event.preventDefault();
      // Reset our form values
      this.form.title = "";
      this.form.isbn = "";
      this.form.publishedDate = "";
      this.form.thumbnailUrl = "";
      this.form.descr = "";
      // Trick to reset/clear native browser form validation state
      this.show = false;
      this.$nextTick(() => {
        this.show = true;
      });
    },
  },
};
</script>